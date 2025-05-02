using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public static class TestDataSeeder
{
    public static void Seed(SchoolDbContext context)
    {
        if (context.Rocniky.Any() || context.Studenti.Any() || context.Predmety.Any())
        {
            Console.WriteLine("❌ Testovací data už v databázi existují, seedování se přeskočilo.");
            return;
        }

        var rand = new Random();
        var today = DateTime.Today;

        var rocniky = new List<(int Cislo, string[] Predmety, List<(string Jmeno, string Prijmeni, DateTime Narozeni)> Studenti)>
        {
            (1, new[] { "Matematika 1", "Fyzika", "Chemie", "Dějepis", "Český jazyk" }, new List<(string, string, DateTime)>
            {
                ("Jan", "Novák", new DateTime(2007, 5, 12)),
                ("Petr", "Svoboda", new DateTime(2007, 8, 30)),
                ("Vašek", "Horák", new DateTime(2007, 8, 11)),
                ("Pepa", "Tomášek", new DateTime(2007, 6, 10)),
                ("Jan", "Flek", new DateTime(2007, 1, 1)),
                ("Tomáš", "Kříž", new DateTime(2007, 3, 3)),
                ("Jana", "Malá", new DateTime(2007, 11, 20)),
                ("Lucie", "Veselá", new DateTime(2007, 7, 15))
            }),
            (2, new[] { "Matematika 2", "Programování", "Angličtina", "Zeměpis", "Biologie" }, new List<(string, string, DateTime)>
            {
                ("Lucie", "Králová", new DateTime(2006, 3, 22)),
                ("Eva", "Horáková", new DateTime(2006, 10, 14)),
                ("Dominik", "Adam", new DateTime(2006, 11, 1)),
                ("Karel", "Novotný", new DateTime(2006, 5, 5)),
                ("Petra", "Černá", new DateTime(2006, 9, 9)),
                ("Michal", "Svoboda", new DateTime(2006, 12, 2)),
                ("Klára", "Veselá", new DateTime(2006, 4, 18)),
                ("Ondřej", "Blažek", new DateTime(2006, 6, 30))
            }),
            (3, new[] { "Matematika 3", "Databáze", "Němčina", "Fyzika 2", "Občanská nauka" }, new List<(string, string, DateTime)>
            {
                ("Tomáš", "Dvořák", new DateTime(2005, 2, 17)),
                ("Anna", "Benešová", new DateTime(2005, 6, 5)),
                ("David", "Musil", new DateTime(2005, 8, 25)),
                ("Barbora", "Nováková", new DateTime(2005, 10, 12)),
                ("Jakub", "Pokorný", new DateTime(2005, 1, 30)),
                ("Eva", "Doležalová", new DateTime(2005, 12, 8))
            }),
            (4, new[] { "Matematika 4", "Sítě", "Softwarové inženýrství", "Literatura", "Ekonomie" }, new List<(string, string, DateTime)>
            {
                ("Martin", "Kolář", new DateTime(2004, 9, 9)),
                ("Barbora", "Sedláčková", new DateTime(2004, 12, 21)),
                ("Jan", "Čech", new DateTime(2004, 2, 14)),
                ("Tereza", "Procházková", new DateTime(2004, 5, 27)),
                ("Filip", "Krejčí", new DateTime(2004, 11, 3)),
                ("Kristýna", "Urbanová", new DateTime(2004, 7, 19))
            })
        };

        foreach (var (cislo, predmety, studenti) in rocniky)
        {
            var rocnik = new Rocnik { Cislo = cislo };
            context.Rocniky.Add(rocnik);
            context.SaveChanges();

            foreach (var nazevPredmetu in predmety)
            {
                context.Predmety.Add(new Predmet
                {
                    Nazev = nazevPredmetu,
                    RocnikId = rocnik.Id
                });
            }
            context.SaveChanges();

            foreach (var (jmeno, prijmeni, narozeni) in studenti)
            {
                context.Studenti.Add(new Student
                {
                    Jmeno = jmeno,
                    Prijmeni = prijmeni,
                    DatumNarozeni = narozeni,
                    RocnikId = rocnik.Id
                });
            }
            context.SaveChanges();

            var studentiVrocniku = context.Studenti.Where(s => s.RocnikId == rocnik.Id).ToList();
            var predmetyVrocniku = context.Predmety.Where(p => p.RocnikId == rocnik.Id).ToList();

            foreach (var student in studentiVrocniku)
            {
                int pocetZnamek = rand.Next(4, 9);
                var poradiPool = Enumerable.Range(1, 8)
                                           .OrderBy(_ => rand.Next())
                                           .ToList();

                for (int i = 0; i < pocetZnamek; i++)
                {
                    var vybranyPredmet = predmetyVrocniku[rand.Next(predmetyVrocniku.Count)];

                    int poradi = poradiPool[i];

                    context.Znamky.Add(new Znamka
                    {
                        Hodnota = rand.Next(1, 6),
                        Datum = today,
                        Poradi = poradi,
                        StudentId = student.Id,
                        PredmetId = vybranyPredmet.Id
                    });
                }
            }

            context.SaveChanges();
        }

        Console.WriteLine("Seedování dokončeno.");
    }
}
