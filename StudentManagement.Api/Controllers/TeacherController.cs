using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Shared.DTOs;

namespace StudentManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Ucitel,Admin")]
public class TeacherController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public TeacherController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet("grades")]
    public async Task<ActionResult<List<RocnikDto>>> GetGrades()
    {
        var rocniky = await _context.Rocniky
            .OrderBy(r => r.Cislo)
            .Select(r => new RocnikDto
            {
                Id = r.Id,
                Cislo = r.Cislo
            })
            .ToListAsync();

        return Ok(rocniky);
    }

    [HttpGet("subjects")]
    public async Task<ActionResult<List<PredmetDto>>> GetSubjects()
    {
        var subjects = await _context.Predmety
            .Select(p => new PredmetDto
            {
                Id = p.Id,
                Nazev = p.Nazev,
                RocnikId = p.RocnikId
            })
            .ToListAsync();

        return Ok(subjects);
    }

    [HttpGet("students/{rocnikId}")]
    public async Task<ActionResult<List<StudentDto>>> GetStudentsByGrade(int rocnikId)
    {
        var studenti = await _context.Studenti
            .Where(s => s.RocnikId == rocnikId)
            .Include(s => s.Znamky)
                .ThenInclude(z => z.Predmet)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                Jmeno = s.Jmeno,
                Prijmeni = s.Prijmeni,
                DatumNarozeni = s.DatumNarozeni,
                RocnikId = s.RocnikId,
                Rocnik = s.Rocnik.Cislo,
                Znamky = s.Znamky
                    .OrderBy(z => z.Poradi)
                    .Select(z => new ZnamkaDto
                    {
                        Id = z.Id,
                        Hodnota = z.Hodnota,
                        PredmetId = z.PredmetId,
                        PredmetNazev = z.Predmet.Nazev,
                        Datum = z.Datum,
                        Poradi = z.Poradi
                    }).ToList()
            })
            .ToListAsync();

        return Ok(studenti);
    }

    [HttpPost("grades/save")]
    public async Task<IActionResult> SaveGrades([FromBody] SaveGradesRequestDto request)
    {
        if (request.SubjectId <= 0 || request.Grades == null || request.Grades.Count == 0)
            return BadRequest("Neplatné vstupní údaje.");

        var studentIds = request.Grades.Select(g => g.StudentId).ToList();

        var existingGrades = await _context.Znamky
            .Where(z => z.PredmetId == request.SubjectId && studentIds.Contains(z.StudentId))
            .ToListAsync();

        var newGrades = new List<Znamka>();

        foreach (var student in request.Grades)
        {
            var existingForStudent = existingGrades
                .Where(z => z.StudentId == student.StudentId)
                .ToList();

            for (int i = 0; i < 10; i++)
            {
                var input = student.Znamky.ElementAtOrDefault(i);
                var existing = existingForStudent.FirstOrDefault(z => z.Poradi == i);

                if (input.HasValue && (input < 1 || input > 5))
                    return BadRequest($"Známka musí být mezi 1 a 5 (student ID {student.StudentId}).");

                if (input.HasValue)
                {
                    if (existing != null)
                    {
                        if (existing.Hodnota != input.Value)
                        {
                            existing.Hodnota = input.Value;
                            existing.Datum = DateTime.Now;
                            _context.Znamky.Update(existing);
                        }
                    }
                    else
                    {
                        newGrades.Add(new Znamka
                        {
                            StudentId = student.StudentId,
                            PredmetId = request.SubjectId,
                            Hodnota = input.Value,
                            Datum = DateTime.Now,
                            Poradi = i
                        });
                    }
                }
                else
                {
                    if (existing != null)
                        _context.Znamky.Remove(existing);
                }
            }
        }

        if (newGrades.Any())
            _context.Znamky.AddRange(newGrades);

        await _context.SaveChangesAsync();
        return Ok("Známky byly uloženy.");
    }

    [HttpGet("students/{rocnikId}/subject/{subjectId}")]
    public async Task<ActionResult<List<StudentDto>>> GetStudentsByGradeAndSubject(int rocnikId, int subjectId)
    {
        var studenti = await _context.Studenti
            .Where(s => s.RocnikId == rocnikId)
            .Include(s => s.Znamky.Where(z => z.PredmetId == subjectId))
                .ThenInclude(z => z.Predmet)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                Jmeno = s.Jmeno,
                Prijmeni = s.Prijmeni,
                DatumNarozeni = s.DatumNarozeni,
                RocnikId = s.RocnikId,
                Rocnik = s.Rocnik.Cislo,
                Znamky = s.Znamky
                    .Where(z => z.PredmetId == subjectId)
                    .OrderBy(z => z.Poradi)
                    .Select(z => new ZnamkaDto
                    {
                        Id = z.Id,
                        Hodnota = z.Hodnota,
                        PredmetId = z.PredmetId,
                        PredmetNazev = z.Predmet.Nazev,
                        Datum = z.Datum,
                        Poradi = z.Poradi
                    }).ToList()
            })
            .ToListAsync();

        return Ok(studenti);
    }
}
