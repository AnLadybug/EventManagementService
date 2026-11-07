using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ru.AnLadybug.EventManagement.Middlewares;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger;

    //  Создайте middleware для глобальной обработки исключений:
    //  middleware перехватывает все необработанные исключения;
    //  возвращается единообразный JSON - ответ с информацией об ошибке;
    //  формат ответа содержит: код статуса, сообщение об ошибке;
    //  рекомендуем использовать Problem Details(RFC 7807) или собственный формат.

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

            await WriteResponse(httpContext, ex);
        }        
    }

    private static async Task WriteResponse(HttpContext httpContext, Exception ex)
    {
        var statusCode = GetStatusCode(ex);
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Type = "about:blank",
            Title = GetTitleForStatus(ex),
            Status = statusCode,
            Detail = ex.Message,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }

    private static int GetStatusCode(Exception ex) =>
        ex switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitleForStatus(Exception ex) =>
        ex switch
        {
            ValidationException => "Ошибка валидации данных",
            BadHttpRequestException => "Некорректный формат запроса",
            _ => "Внутренняя ошибка сервера"
        };
}
