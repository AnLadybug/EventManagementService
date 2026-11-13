using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ru.AnLadybug.EventManagement.Middlewares;

public class GlobalExceptionHandlingMiddleware(
    RequestDelegate next, 
    ILogger<GlobalExceptionHandlingMiddleware> logger,
    IWebHostEnvironment webHostEnv)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;
    private readonly IWebHostEnvironment _webHostEnv = webHostEnv;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                    ex,
                    "Необработанное исключение: Method={Method}, Path={Path}",
                    httpContext.Request.Method,
                    httpContext.Request.Path
            );

            if (httpContext.Response.HasStarted)
            {
                _logger.LogWarning("Ответ уже начал отправляться клиенту. Невозможно изменить заголовки для Problem Details.");
                return;
            }

            await WriteResponse(httpContext, ex, _webHostEnv);
        }
    }

    private static async Task WriteResponse(HttpContext httpContext, Exception ex, IWebHostEnvironment webHostEnv)
    {
        var statusCode = GetStatusCode(ex);
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Type = "about:blank",
            Title = GetTitleForStatus(ex),
            Status = statusCode,
            Detail = GetDetailForStatus(webHostEnv, statusCode, ex.Message),
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, httpContext.RequestAborted);
    }

    private static int GetStatusCode(Exception ex) =>
        ex switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitleForStatus(Exception ex) =>
        ex switch
        {
            ValidationException => "Ошибка валидации данных",
            BadHttpRequestException => "Некорректный формат запроса",
            KeyNotFoundException => "Ресурс не найден",
            _ => "Непредвиденная ошибка сервера"
        };

    private static string GetDetailForStatus(IWebHostEnvironment webHostEnv, int statusCode, string message) => 
        !webHostEnv.IsDevelopment() && statusCode == StatusCodes.Status500InternalServerError 
            ? "Непредвиденная ошибка сервера. Попробуйте повторить запрос позже."
            : message;
    
}
