using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services.Database;
using StudentManagement.Shared.Auth;
using StudentManagement.Shared.DTOs;
using System;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public AdminController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpPost("seed-test-data")]
        public IActionResult SeedTestData()
        {
            TestDataSeeder.Seed(_context);
            return Ok("Databáze byla naplněna testovacími daty.");
        }

        [HttpDelete("clear")]
        public IActionResult ClearDb()
        {
            _context.Znamky.RemoveRange(_context.Znamky);
            _context.Studenti.RemoveRange(_context.Studenti);
            _context.Predmety.RemoveRange(_context.Predmety);
            _context.Rocniky.RemoveRange(_context.Rocniky);
            _context.SaveChanges();

            return Ok("Databáze byla vymazána.");
        }

        [HttpPost("create-user")]
        public IActionResult CreateUser(CreateUserDto dto)
        {

            try
            {
                if (_context.Uzivatele.Any(u => u.UzivatelskeJmeno == dto.UzivatelskeJmeno))
                {
                    return BadRequest("Uživatel již existuje");
                }

                var user = new Uzivatel
                {
                    UzivatelskeJmeno = dto.UzivatelskeJmeno,
                    HesloHash = BCrypt.Net.BCrypt.HashPassword(dto.Heslo),
                    Role = dto.Role
                };

                _context.Uzivatele.Add(user);
                _context.SaveChanges();

                return Ok("Uživatel vytvořen.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Chyba na serveru při vytváření uživatele.");
            }
        }

        [HttpPost("add-student")]
        public IActionResult AddStudent(StudentCreateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Jmeno) || string.IsNullOrWhiteSpace(dto.Prijmeni))
                {
                    return BadRequest("Jméno a příjmení jsou povinná pole.");
                }

                var student = new Student
                {
                    Jmeno = dto.Jmeno,
                    Prijmeni = dto.Prijmeni,
                    DatumNarozeni = dto.DatumNarozeni,
                    RocnikId = dto.RocnikId,
                    Znamky = new List<Znamka>()
                };

                _context.Studenti.Add(student);
                _context.SaveChanges();

                foreach (var predmetId in dto.ZvolenePredmety.Distinct())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        _context.Znamky.Add(new Znamka
                        {
                            StudentId = student.Id,
                            PredmetId = predmetId,
                            Poradi = i,
                            Datum = DateTime.Now
                        });
                    }
                }

                _context.SaveChanges();
                return Ok("Student úspěšně přidán.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, "Chyba na serveru při ukládání studenta.");
            }
        }
    }
}
