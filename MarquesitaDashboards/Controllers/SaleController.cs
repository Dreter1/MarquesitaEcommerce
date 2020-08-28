using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
