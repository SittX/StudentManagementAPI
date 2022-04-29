using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI;

public class Student
{
    [Required]
    [Range(1, 99999999999999)]
    public int StudentId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "Name must not be more than 50 words.")]
    public string? Name { get; set; }
    [Required]
    [Range(16, 30, ErrorMessage = "Age must be only between 16 to 30.")]
    public int Age { get; set; }
    [Required]
    [Range(1, 12, ErrorMessage = "Grade must be only between 1 to 12")]
    public int Grade { get; set; }
    public double GPA { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}