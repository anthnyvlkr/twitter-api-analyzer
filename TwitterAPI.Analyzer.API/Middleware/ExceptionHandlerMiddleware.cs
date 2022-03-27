using System.Net;
using System.Text.Json;

namespace TwitterAPI.Analyzer.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int) HttpStatusCode.InternalServerError;
            var result = JsonSerializer.Serialize(new { message = e?.Message });
            await response.WriteAsync(result);
        }
    }
}
