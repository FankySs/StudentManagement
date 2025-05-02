namespace StudentManagement.Shared.DTOs
{
    public class StudentGradeInputDto
    {
        public int StudentId { get; set; }
        public string Jmeno { get; set; } = string.Empty;
        public string Prijmeni { get; set; } = string.Empty;
        public int?[] Znamky { get; set; } = new int?[10];
    }
}
