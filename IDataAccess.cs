using StudentManagementAPI;

public interface IDataAccess
{
    bool TryGetStudentsData(out List<Student> students);
    Student AddNewStudent(Student newStudent);
    List<Student> RemoveStudent(int studentId);

}