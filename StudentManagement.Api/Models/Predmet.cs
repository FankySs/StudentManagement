namespace StudentManagement.Api.Models
{
    public class Predmet
    {
        public int Id { get; set; }
        public string Nazev { get; set; }

        public int RocnikId { get; set; }
        public Rocnik Rocnik { get; set; }

        public ICollection<Znamka> Znamky { get; set; }
    }
}
