using StudentManagement.Api.Data;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services.Database
{
    public static class DbSeeder
    {
        public static void Seed(SchoolDbContext context, IConfiguration config)
        {
            //context.Database.EnsureDeleted(); 
            //use for deleting DB
            context.Database.EnsureCreated();

            if (!context.Uzivatele.Any(u => u.Role == "Admin"))
            {
                var username = config["DefaultAdmin:Username"];
                var password = config["DefaultAdmin:Password"];

                var admin = new Uzivatel
                {
                    UzivatelskeJmeno = username,
                    HesloHash = BCrypt.Net.BCrypt.HashPassword(password),
                    Role = "Admin"
                };

                context.Uzivatele.Add(admin);
                context.SaveChanges();

                Console.WriteLine("Admin účet byl vytvořen z konfigurace.");
            }
        }
    }
}
