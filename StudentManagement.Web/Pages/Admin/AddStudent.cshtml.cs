using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Shared.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StudentManagement.Web.Pages.Admin
{
    public class AddStudentModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddStudentModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public StudentCreateDto NewStudent { get; set; } = new();

        public List<RocnikDto> AvailableGrades { get; set; } = new();
        public List<PredmetDto> AvailableSubjects { get; set; } = new();

        public SelectList GradesSelectList => new(AvailableGrades, "Id", "Cislo");

        public string StatusMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            await LoadData(token);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            var client = _httpClientFactory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("api/admin/add-student", NewStudent);

            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "Student byl úspěšně přidán.";
                NewStudent = new();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                StatusMessage = $"Nepodařilo se uložit studenta: {error}";
            }

            await LoadData(token);
            return Page();
        }

        private async Task LoadData(string token)
        {
            var client = _httpClientFactory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var grades = await client.GetFromJsonAsync<List<RocnikDto>>("api/teacher/grades");
            AvailableGrades = grades ?? new();

            var subjects = await client.GetFromJsonAsync<List<PredmetDto>>("api/teacher/subjects");
            AvailableSubjects = subjects ?? new();
        }
    }
}
