namespace StudentManagement.Shared.DTOs
{
    public class ZnamkaDto
    {
        public int Id { get; set; }
        public int PredmetId { get; set; }
        public string PredmetNazev { get; set; }
        public int Hodnota { get; set; }
        public DateTime Datum { get; set; }
    }
}
