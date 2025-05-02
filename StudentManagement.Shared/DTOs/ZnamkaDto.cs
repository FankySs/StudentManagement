namespace StudentManagement.Shared.DTOs
{
    public class ZnamkaDto
    {
        public int Id { get; set; }
        public int PredmetId { get; set; }
        public int StudentId { get; set; }
        public string PredmetNazev { get; set; }
        public int Hodnota { get; set; }
        public DateTime Datum { get; set; }
        public int Poradi { get; set; }

    }
}
