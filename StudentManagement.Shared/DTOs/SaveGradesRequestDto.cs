using StudentManagement.Shared.DTOs;

public class SaveGradesRequestDto
{
    public int SubjectId { get; set; }
    public List<StudentGradeInputDto> Grades { get; set; } = new();
}
