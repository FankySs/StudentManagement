namespace StudentManagement.Api.Models
{
    public class Znamka
    {
        public int Id { get; set; }
        public int Hodnota { get; set; }
        public DateTime Datum { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; }

        public int Poradi { get; set; }
    }
}
