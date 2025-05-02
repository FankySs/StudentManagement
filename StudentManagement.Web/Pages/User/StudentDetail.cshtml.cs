using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagement.Shared.DTOs;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.Web.Pages.User
{
    public class StudentDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const int MaxColumns = 10;

        public StudentDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public StudentDto Student { get; set; }
        public Dictionary<string, int?[]> ZnamkyPoPredmetech { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            var client = _httpClientFactory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/student/{Id}");
            if (!response.IsSuccessStatusCode)
                return RedirectToPage("/User/Overview");

            Student = await response.Content.ReadFromJsonAsync<StudentDto>();

            ZnamkyPoPredmetech = Student.Znamky
                .GroupBy(z => z.PredmetNazev)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var arr = new int?[MaxColumns];
                        foreach (var z in g)
                        {
                            int idx = z.Poradi;
                            if (idx >= 0 && idx < MaxColumns)
                                arr[idx] = z.Hodnota;
                        }
                        return arr;
                    }
                );

            return Page();
        }
    }
}