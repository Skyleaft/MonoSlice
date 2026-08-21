using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonoSlice.Shared.Abstractions.Common;
using MonoSlice.Shared.Abstractions.Exceptions;

using MonoSlice.Shared.Infrastructure.Serialization;

namespace MonoSlice.Shared.Infrastructure.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var env = context.RequestServices.GetService<IHostEnvironment>();
        var isDevelopment = env?.IsDevelopment() ?? false;

        var (statusCode, response) = exception switch
        {
            NotFoundException notFound => (
                HttpStatusCode.NotFound,
                ApiResponse.Fail(notFound.Message)),

            ValidationException validation => (
                HttpStatusCode.BadRequest,
                ApiResponse.Fail(validation.Message, validation.Errors)),

            BusinessRuleException business => (
                HttpStatusCode.UnprocessableEntity,
                ApiResponse.Fail(business.Message)),

            ForbiddenException forbidden => (
                HttpStatusCode.Forbidden,
                ApiResponse.Fail(forbidden.Message)),

            UnauthorizedAccessException unauthorized => (
                HttpStatusCode.Unauthorized,
                ApiResponse.Fail(unauthorized.Message)),

            _ => (
                HttpStatusCode.InternalServerError,
                ApiResponse.Fail(isDevelopment
                    ? $"An unexpected error occurred: [{exception.GetType().Name}] {exception.Message} | Stack: {exception.StackTrace}"
                    : "An unexpected error occurred. Please try again later."))
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning("Handled domain exception [{StatusCode}]: {Message}", statusCode, exception.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, SharedJsonSerializerContext.DefaultOptions));
    }
}
