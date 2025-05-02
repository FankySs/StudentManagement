using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Shared.DTOs;
using System.Net.Http.Headers;

namespace StudentManagement.Web.Pages.User
{
    public class GradesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GradesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int SelectedGradeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SelectedSubjectId { get; set; }

        [BindProperty]
        public List<StudentGradeInputDto> StudentGrades { get; set; } = new();

        public List<RocnikDto> AvailableGrades { get; set; } = new();
        public List<PredmetDto> Subjects { get; set; } = new();

        public string StatusMessage { get; set; } = string.Empty;

        public SelectList GradesSelectList => new(AvailableGrades, "Id", "Cislo");
        public SelectList SubjectsSelectList => new(Subjects, "Id", "Nazev");

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            var client = _httpClientFactory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var gradesResponse = await client.GetAsync("api/teacher/grades");
            if (gradesResponse.IsSuccessStatusCode)
                AvailableGrades = await gradesResponse.Content.ReadFromJsonAsync<List<RocnikDto>>() ?? new();

            var subjectsResponse = await client.GetAsync("api/teacher/subjects");
            if (subjectsResponse.IsSuccessStatusCode)
            {
                var allSubjects = await subjectsResponse.Content.ReadFromJsonAsync<List<PredmetDto>>() ?? new();

                if (SelectedGradeId > 0)
                    Subjects = allSubjects.Where(s => s.RocnikId == SelectedGradeId).ToList();
                else
                    Subjects = allSubjects;
            }

            if (SelectedGradeId > 0 && SelectedSubjectId > 0)
            {
                var studentsResponse = await client.GetAsync($"api/teacher/students/{SelectedGradeId}/subject/{SelectedSubjectId}");
                if (studentsResponse.IsSuccessStatusCode)
                {
                    var students = await studentsResponse.Content.ReadFromJsonAsync<List<StudentDto>>() ?? new();
                    StudentGrades = students.Select(s => new StudentGradeInputDto
                    {
                        StudentId = s.Id,
                        Jmeno = s.Jmeno,
                        Prijmeni = s.Prijmeni,
                        Znamky = Enumerable.Range(0, 10)
                            .Select(i => (int?)s.Znamky.FirstOrDefault(z => z.Poradi == i)?.Hodnota)
                            .ToArray()
                    }).ToList();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            var client = _httpClientFactory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            foreach (var student in StudentGrades)
            {
                for (int i = 0; i < student.Znamky.Length; i++)
                {
                    var val = student.Znamky[i];
                    if (val.HasValue && (val < 1 || val > 5))
                        student.Znamky[i] = null;
                }
            }

            var request = new SaveGradesRequestDto
            {
                SubjectId = SelectedSubjectId,
                Grades = StudentGrades
            };

            var response = await client.PostAsJsonAsync("api/teacher/grades/save", request);

            if (response.IsSuccessStatusCode)
            {
                TempData["StatusMessage"] = "Známky byly úspìšnì uloženy.";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["StatusMessage"] = $"Chyba pøi ukládání známek: {error}";
            }

            return RedirectToPage(new { SelectedGradeId, SelectedSubjectId });
        }
    }
}
