using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentManagementAPI.Filters;

public class Students_CheckAPIVersionResourceFilter : IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        using (StreamWriter sw = new StreamWriter("Logging.txt", append: true))
        {
            sw.WriteLine($"[ {context.HttpContext.TraceIdentifier} ] {context.HttpContext.Connection} : {context.HttpContext.GetEndpoint().ToString()}");
        }
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (context.HttpContext.Request.Path.Value.ToLower().Contains("v2"))
        {
            context.ModelState.AddModelError("API versioning Error", "The requested API version is deprecated. Please upgrade into new version.");

            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
            };

            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}