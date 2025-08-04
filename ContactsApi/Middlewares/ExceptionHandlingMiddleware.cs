using System.Net;
using ContactsApi.Exceptions;

namespace ContactsApi.Middlewares;

public class ExceptionHandlingMiddleware(
    ILogger<ExceptionHandlingMiddleware> logger,
    RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Anhandled exception occured");

            (int code, string message) = ex switch
            {
                CustomConflictException conflict => ((int)HttpStatusCode.Conflict, conflict.Message),
                CustomNotFoundException notFound => ((int)HttpStatusCode.NotFound, notFound.Message),
                _ => ((int)HttpStatusCode.InternalServerError, ex.Message)
            };

            context.Response.StatusCode = code;
            await context.Response.WriteAsync(message);
        }
    }
}