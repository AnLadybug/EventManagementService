using Ru.AnLadybug.EventManagement.Middlewares;

namespace Ru.AnLadybug.EventManagement.Extensions;

public static class GlobalExceptionHandlingMiddlewareExtension
{
    public static IApplicationBuilder UseGlobalExceptionHandling(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
