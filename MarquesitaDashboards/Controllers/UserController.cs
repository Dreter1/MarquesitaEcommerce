using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        [Authorize(Policy = "CanViewUsers")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
