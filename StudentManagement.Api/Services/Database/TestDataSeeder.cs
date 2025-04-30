using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using System;
using System.Collections.Generic;

namespace StudentManagement.Api.Services.Database
{
    public static class TestDataSeeder
    {
        public static void Seed(SchoolDbContext context)
        {
            if (context.Rocniky.Any())
                return;

            for (int r = 1; r <= 4; r++)
            {
                var rocnik = new Rocnik { Cislo = r };
                context.Rocniky.Add(rocnik);
                context.SaveChanges();

                var trida = new Trida
                {
                    Nazev = $"{r}.A",
                    Kapacita = 30
                };
                context.Tridy.Add(trida);
                context.SaveChanges();

                for (int i = 1; i <= 10; i++)
                {
                    context.Studenti.Add(new Student
                    {
                        Jmeno = $"Student{r}{i}",
                        Prijmeni = $"Prijmeni{r}{i}",
                        DatumNarozeni = DateTime.Now.AddYears(-15).AddDays(i),
                        RocnikId = rocnik.Id,
                        TridaId = trida.Id
                    });
                }

                for (int p = 1; p <= 5; p++)
                {
                    context.Predmety.Add(new Predmet
                    {
                        Nazev = $"Predmet {r}.{p}",
                        RocnikId = rocnik.Id
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
