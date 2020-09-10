using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        [Authorize(Policy = "Client")]
        public IActionResult MyProfile()
        {
            return View();
        }

        [Authorize(Policy = "Client")]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
