using Claims.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Claims.Infrastructure.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        if (exception is RequestValidationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }

        if (exception is DataNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }

        // Other exceptions can be handled differently
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return context.Response.WriteAsync(new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = "An internal server error has occurred."
        }.ToString());
    }

    private class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = default!;

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
