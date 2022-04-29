using System;
using System.Text.Json;
namespace StudentManagementAPI;

/// <summary>
/// This is class where all the data access to the StudentData happens
/// All the methods should be void and return Boolean value. To return data we need to use output parameter.
/// </summary>
public class StudentDataAccess : IDataAccess
{
    public bool TryGetStudentsData(out List<Student> students)
    {
        try
        {
            // Instantiate students list
            students = new List<Student>();
            // Read data from the text file
            using (StreamReader sr = new StreamReader("StudentsData.txt"))
            {

                while (!sr.EndOfStream)
                {
                    // Deserialize each line into student object
                    Student? student = JsonSerializer.Deserialize<Student>(sr.ReadLine());
                    if (student != null)
                    {
                        students.Add(student);
                    }
                }
            }
            return true;

        }
        catch (FormatException fe)
        {
            Console.WriteLine(fe.Message);
            students = null;
            return false;
        }
        catch (System.Exception se)
        {
            Console.WriteLine(se.Message);
            students = null;
            return false;
        }
    }

    public Student? AddNewStudent(Student newStudent)
    {
        try
        {
            // Serialize the incoming student object into json string
            string jsonString = JsonSerializer.Serialize(newStudent);
            using (StreamWriter sw = new StreamWriter("StudentsData.txt", append: true))
            {
                sw.WriteLine(jsonString);
            }
            return newStudent;
        }
        catch (FileNotFoundException fx)
        {
            Console.WriteLine(fx.Message);
            return null;
        }
        catch (FormatException fe)
        {
            Console.WriteLine($"Format exception : {fe.Message}");
            return null;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.Message);
            return null;
        }

    }

    public List<Student> RemoveStudent(int studentId)
    {
        List<Student> studentsBuffer = new List<Student>();
        List<Student> students;
        try
        {

            using (StreamReader sr = new StreamReader("StudentsData.txt"))
            {
                while (!sr.EndOfStream)
                {
                    studentsBuffer.Add(JsonSerializer.Deserialize<Student>(sr.ReadLine()));
                }

            }

            using (StreamWriter sw = new StreamWriter("StudentsData.txt"))
            {

                students = (from student in studentsBuffer
                            where student.StudentId != studentId
                            select student).ToList<Student>();
                foreach (var student in students)
                {
                    sw.WriteLine(JsonSerializer.Serialize(student));
                }

            }

            return students;
        }
        catch (FormatException fe)
        {
            Console.WriteLine(fe.Message);
            Console.WriteLine($"Stack track for {fe.Message} error : {fe.StackTrace}");
            return null;
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Stack track for {e.Message} error : {e.StackTrace}");
            return null;
        }

    }
}
