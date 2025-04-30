namespace StudentManagement.Api.Models
{
    public class Uzivatel
    {
        public int Id { get; set; }
        public string UzivatelskeJmeno { get; set; }
        public string HesloHash { get; set; }
        public string Role { get; set; } // "Admin", "Ucitel"
    }
}
