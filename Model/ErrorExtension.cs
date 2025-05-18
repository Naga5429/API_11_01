using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

public class ErrorExtension : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Log the exception (e.g., using Serilog, NLog, etc.)
        Console.WriteLine(context.Exception.Message);

        // Create a custom response
        var response = new
        {
            ErrorMessage = "An unexpected error occurred.",
            Exception = context.Exception.Message, // Include this for debugging; remove in production
            StatusCode = 500
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = 500
        };

        // Mark the exception as handled
        context.ExceptionHandled = true;
    }
}
