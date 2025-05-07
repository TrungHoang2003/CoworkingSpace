using System.Diagnostics;
using System.Net;

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
           var response = new ExceptionResponse
           {
               Message = ex.Message,
               StackTrace = ex.StackTrace
           };

           await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}

public class ExceptionResponse
{
    public string Code { get; set; } = "Internal Server Error";
    public string Message { get; set; }
    public string? StackTrace { get; set; }
}