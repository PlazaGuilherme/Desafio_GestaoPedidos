using GestaoPedido.UI.Server.Middleware;

namespace GestaoPedido.UI.Server.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomException(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
