using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StudentManagement.Web.Pages.User
{
    public class DashboardModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string StatusMessage { get; set; }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Auth/Login");
        }
    }
}
