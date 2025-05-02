using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Shared.DTOs;
using System.Net.Http.Headers;

namespace StudentManagement.Web.Pages.User
{
    public class OverviewModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OverviewModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int SelectedGradeId { get; set; }

        public List<StudentDto> Students { get; set; } = new();
        public List<RocnikDto> DostupneRocniky { get; set; } = new();

        public SelectList RocnikySelectList => new(DostupneRocniky, "Id", "Cislo");

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            var client = _httpClientFactory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var rocnikyResponse = await client.GetAsync("api/teacher/grades");
            if (rocnikyResponse.IsSuccessStatusCode)
            {
                DostupneRocniky = await rocnikyResponse.Content.ReadFromJsonAsync<List<RocnikDto>>() ?? new();
            }

            if (SelectedGradeId > 0)
            {
                var studentiResponse = await client.GetAsync($"api/teacher/students/{SelectedGradeId}");
                if (studentiResponse.IsSuccessStatusCode)
                {
                    Students = await studentiResponse.Content.ReadFromJsonAsync<List<StudentDto>>() ?? new();
                }
            }

            return Page();
        }
    }
}
