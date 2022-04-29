using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Filters;

namespace StudentManagementAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StudentsController : ControllerBase
{
    // Inject dependency into the controller constructor 
    private readonly IDataAccess _dataAccess;
    public StudentsController(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }


    [HttpGet]
    public IActionResult GetAllStudents()
    {
        if (_dataAccess.TryGetStudentsData(out List<Student> students))
        {
            return Ok(students);
        }
        return BadRequest();
    }


    [HttpPost]
    // There might be a validationAttribute here
    [ServiceFilter(typeof(Students_DuplicationCheckActionFilter))]
    // [Students_DuplicationCheckActionFilter]
    public IActionResult CreateNewStudent(Student newStudent)
    {
        Student student = _dataAccess.AddNewStudent(newStudent);
        if (student != null)
        {
            return Ok(student);

        }
        return BadRequest();
    }

    [HttpPut]
    public IActionResult UpdateStudent([FromBody] Student newStudent, [FromQuery] int studentId)
    {
        if (_dataAccess.TryGetStudentsData(out List<Student> students))
        {
            if (students != null)
            {
                IEnumerable<Student> StudentThatNeedToUpdate = from student in students
                                                               where student.StudentId == studentId
                                                               select student;

                foreach (var student in StudentThatNeedToUpdate)
                {
                    _dataAccess.RemoveStudent(student.StudentId);
                    student.StudentId = newStudent.StudentId;
                    student.Name = newStudent.Name;
                    student.Email = newStudent.Email;
                    student.GPA = newStudent.GPA;
                    student.Grade = newStudent.Grade;
                    _dataAccess.AddNewStudent(student);
                }

            }
        }
        return CreatedAtAction("UpdateStudent", students);
    }

    [HttpDelete]
    public IActionResult DeleteStudent(int studentId)
    {
        List<Student> students = _dataAccess.RemoveStudent(studentId);
        if (students.Count() > 0)
        {
            return Ok(students);
        }
        else
        {
            return BadRequest();
        }

    }
}