using System.Text.Json;

namespace PowerUp.Api.Middlewares;

public class GlobalExceptionHandling : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandling> _logger;

    public GlobalExceptionHandling(ILogger<GlobalExceptionHandling> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            var errorResponse = new ErrorResponse(ex.Message, context.Response.StatusCode, ex.StackTrace);
            
            var errorJson = JsonSerializer.Serialize(errorResponse);
            context.Response.ContentType = "application/json";

            await using var sw = new StreamWriter(context.Response.Body);
            await sw.WriteAsync(errorJson);
        }
    }
    
    private record ErrorResponse(string Message, int StatusCode, string? Details);
}