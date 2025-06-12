using Products.Utility.Exceptions;
using System.Net;
using System.Text.Json;

namespace Products.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RepositoryException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { status = ex.StatusCode, error = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { status = 500, error = "An unexpected error occurred." });
                await context.Response.WriteAsync(result);
            }
        }
    }
}