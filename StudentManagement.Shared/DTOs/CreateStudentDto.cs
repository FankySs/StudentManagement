namespace StudentManagement.Shared.DTOs
{
    public class StudentCreateDto
    {
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public DateTime DatumNarozeni { get; set; }
        public int RocnikId { get; set; }
        public List<int> ZvolenePredmety { get; set; } = new();
    }
}