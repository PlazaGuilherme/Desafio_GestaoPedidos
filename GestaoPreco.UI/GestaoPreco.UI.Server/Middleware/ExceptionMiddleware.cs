using GestaoPedido.Infrastructure.Logging;
using Infrastructure;

namespace GestaoPedido.UI.Server.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IErrorLogRepository errorRepository)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                var statusCode = StatusCodes.Status500InternalServerError;

                await errorRepository.SaveAsync(new ErrorLog
                {
                    Id = Guid.NewGuid(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace ?? "",
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    StatusCode = statusCode,
                    CreatedAt = DateTime.UtcNow
                });

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode,
                    message = "Erro interno do servidor"
                });
            }
        }
    }
}
