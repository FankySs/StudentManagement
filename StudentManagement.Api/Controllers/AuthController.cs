using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Api.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
