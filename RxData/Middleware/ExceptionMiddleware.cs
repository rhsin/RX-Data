using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RxData.DTO;
using System;
using System.Threading.Tasks;

namespace RxData.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(new Error
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                }
                .ToString());
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
