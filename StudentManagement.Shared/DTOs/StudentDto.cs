namespace StudentManagement.Shared.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public DateTime DatumNarozeni { get; set; }

        public int TridaId { get; set; }
        public int RocnikId { get; set; }

        public List<ZnamkaDto> Znamky { get; set; } = new();
    }
}
