using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagement.Shared.Auth;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace StudentManagement.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginRequestDto LoginData { get; set; } = new();

        public string Chyba { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("api");

            var response = await client.PostAsJsonAsync("api/auth/login", LoginData);

            if (!response.IsSuccessStatusCode)
            {
                Chyba = "Neplatné přihlašovací údaje.";
                return Page();
            }

            var loginResult = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            HttpContext.Session.SetString("Token", loginResult.Token);
            HttpContext.Session.SetString("Role", loginResult.Role);
            HttpContext.Session.SetString("Username", loginResult.UzivatelskeJmeno);

            return loginResult.Role switch
            {
                "Admin" => RedirectToPage("/Admin/Dashboard"),
                "Ucitel" => RedirectToPage("/User/Dashboard"),
                _ => RedirectToPage("/Error")
            };
        }
    }
}
