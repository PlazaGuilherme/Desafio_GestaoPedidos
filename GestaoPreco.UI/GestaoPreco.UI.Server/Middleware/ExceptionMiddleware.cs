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

                try
                {
                    await errorRepository.SaveAsync(new ErrorLog
                    {
                        Id = Guid.NewGuid(),
                        Message = ex.Message,
                        StackTrace = ex.StackTrace ?? "",
                        Path = context.Request.Path,
                        Method = context.Request.Method,
                        StatusCode = 500,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                catch
                {
                }

                context.Response.StatusCode = 500;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Erro interno"
                });
            }
        }
    }
}
