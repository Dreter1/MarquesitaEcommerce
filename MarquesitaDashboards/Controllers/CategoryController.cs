using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        [Authorize(Policy = "CanViewCategory")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
