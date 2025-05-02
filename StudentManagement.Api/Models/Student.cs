namespace StudentManagement.Api.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public DateTime DatumNarozeni { get; set; }

        public int RocnikId { get; set; }
        public Rocnik Rocnik { get; set; }

        public ICollection<Znamka> Znamky { get; set; }
    }
}
