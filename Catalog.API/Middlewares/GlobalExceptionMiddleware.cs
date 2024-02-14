using System.Net;
using System.Text.Json;

namespace Catalog.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        var stackTrace = string.Empty;
        if (ex is InvalidOperationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
        }
        else
        {
            stackTrace = ex.StackTrace;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }


        var response = new
        {
            error = new
            {
                message = ex.Message,
                trace = stackTrace
            }
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}