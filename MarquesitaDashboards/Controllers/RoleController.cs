using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        [Authorize(Policy = "CanViewRoles")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
