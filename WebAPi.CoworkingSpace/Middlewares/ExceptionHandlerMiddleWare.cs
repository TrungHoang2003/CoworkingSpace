using System.Diagnostics;
using System.Net;
using Domain.ResultPattern;

namespace CoworkingSpace.Middlewares;

public class ExceptionHandlerMiddleWare(RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }

        catch (Exception ex)
        {
            var statusCode = ex switch
            {
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
           
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType= "application/json";

            var response = new ExceptionResponse
            {
                Description= ex.Message,
                StackTrace = ex.StackTrace
            };

            await httpContext.Response.WriteAsJsonAsync(response);
        }
}
}

public class ExceptionResponse
{
    public string Description{ get; set; }
    public string Code { get; set; } = "Internal Server Error";
    public string? StackTrace { get; set; }
}