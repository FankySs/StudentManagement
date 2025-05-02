using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Shared.DTOs;

namespace StudentManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Ucitel")]
public class StudentController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public StudentController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentById(int id)
    {
        var student = await _context.Studenti
            .Include(s => s.Rocnik)
            .Include(s => s.Znamky)
                .ThenInclude(z => z.Predmet)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
            return NotFound();

        var dto = new StudentDto
        {
            Id = student.Id,
            Jmeno = student.Jmeno,
            Prijmeni = student.Prijmeni,
            DatumNarozeni = student.DatumNarozeni,
            RocnikId = student.Rocnik.Id,
            Rocnik = student.Rocnik.Cislo,
            Znamky = student.Znamky.Select(z => new ZnamkaDto
            {
                Id = z.Id,
                Hodnota = z.Hodnota,
                PredmetId = z.PredmetId,
                PredmetNazev = z.Predmet.Nazev,
                Datum = z.Datum,
                Poradi = z.Poradi
            }).ToList()
        };

        return Ok(dto);
    }
}
