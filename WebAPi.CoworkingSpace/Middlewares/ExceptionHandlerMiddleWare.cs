using System.Diagnostics;
using System.Net;
using Infrastructure.Common;

namespace CoworkingSpace.Middlewares;

public class ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare> logger)
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
           
           var response = new Error("Internal Server Error", ex.InnerException?.Message);

           await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}