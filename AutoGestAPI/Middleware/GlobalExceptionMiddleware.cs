using AutoGestAPI.Exceptions;
using System.Net;

namespace AutoGestAPI.Middleware
{
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
                await HandleExceptions(context, ex);
            }
        }
        public static Task HandleExceptions(HttpContext context, Exception exception)
        {
            HttpStatusCode status = exception switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            string message = exception.Message;
            string? details = exception.StackTrace;

            context.Response.ContentType = "aplication/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsJsonAsync(new
            {
                status,
                message,
                details
            });
        }
    }
}
