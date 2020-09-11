using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marquesita.WebSite.Controllers
{
    [Authorize]
    public class WishListController : Controller
    {
        [Authorize(Policy = "Client")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
