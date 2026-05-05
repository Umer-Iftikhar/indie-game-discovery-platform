using IndieVault.Middleware;

namespace IndieVault.Extensions
{
    public static class MiddlewareExtensions
    {
    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
