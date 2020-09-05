using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        [Authorize(Policy = "CanViewProducts")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
