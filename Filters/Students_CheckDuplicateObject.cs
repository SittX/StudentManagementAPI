using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentManagementAPI.Filters;

public class Students_DuplicationCheckActionFilter : Attribute, IActionFilter
{
    private readonly IDataAccess _dataAccess;
    public Students_DuplicationCheckActionFilter(IDataAccess StudentDataAccess)
    {
        _dataAccess = StudentDataAccess;
    }


    public void OnActionExecuted(ActionExecutedContext context)
    {
        using (StreamWriter sw = new StreamWriter("ObjectDuplicationLog.txt", append: true))
        {
            sw.WriteLine($"{DateTime.Now}");
            sw.WriteLine($"{context.HttpContext.Request.ContentType} : duplicate object creation");
        }
    }


    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Console.WriteLine(context.ActionArguments["student"]);
        var newStudent = context.ActionArguments["newStudent"] as Student;
        if (_dataAccess.TryGetStudentsData(out List<Student> students))
        {
            foreach (var student in students)
            {
                if (student.StudentId == newStudent.StudentId)
                {
                    context.ModelState.AddModelError("Duplicate object", "Object with the current id already exists");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                    };

                    // Short curcuiting the request pipeline
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }
        }

    }
}