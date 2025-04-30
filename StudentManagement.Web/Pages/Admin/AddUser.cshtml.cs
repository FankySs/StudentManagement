using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagement.Shared.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace StudentManagement.Web.Pages.Admin
{
    public class AddUserModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddUserModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public CreateUserDto NovyUzivatel { get; set; } = new();

        public string StatusMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            var client = _httpClientFactory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("api/admin/create-user", NovyUzivatel);

            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "Uživatel byl úspìšnì vytvoøen.";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                StatusMessage = $"Chyba pøi vytváøení uživatele: {error}";
            }

            return Page();
        }
    }
}
