using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace ECA.DiamondKata.Api.Infrastructure;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (ValidationException exception)
        {
            await HandleValidationException(exception, httpContext);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var trackingId = Guid.NewGuid().ToString();
        logger.LogError(exception, "An unhandled exception has occurred. TrackingId: {TrackingId}", trackingId);
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response =  new
        {
            StatusCode = context.Response.StatusCode,
            Message = "An unexpected error occurred. Please try again later.",
            TrackingId = trackingId
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
    
    private async Task HandleValidationException(ValidationException validationException, HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var result = JsonSerializer.Serialize(new
        {
            message = validationException.Message
        });

        await httpContext.Response.WriteAsync(result);
    }
}
