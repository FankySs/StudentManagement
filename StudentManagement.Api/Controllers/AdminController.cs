using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services.Database;
using StudentManagement.Shared.Auth;
using StudentManagement.Shared.DTOs;

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
            _context.Tridy.RemoveRange(_context.Tridy);
            _context.Rocniky.RemoveRange(_context.Rocniky);
            _context.SaveChanges();

            return Ok("Databáze byla vymazána.");
        }

        [HttpPost("create-user")]
        public IActionResult CreateUser(CreateUserDto dto)
        {
            if (_context.Uzivatele.Any(u => u.UzivatelskeJmeno == dto.UzivatelskeJmeno))
                return BadRequest("Uživatel již existuje");

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

    }
}
