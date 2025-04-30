namespace StudentManagement.Api.Models
{
    public class Trida
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
        public int Kapacita { get; set; }

        public ICollection<Student> Studenti { get; set; }
    }
}
