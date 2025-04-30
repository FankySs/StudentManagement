namespace StudentManagement.Api.Models
{
    public class Rocnik
    {
        public int Id { get; set; }
        public int Cislo { get; set; } // 1 až 4

        public ICollection<Student> Studenti { get; set; }
    }
}
