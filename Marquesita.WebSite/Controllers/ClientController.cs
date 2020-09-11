using Microsoft.AspNetCore.Authorization;
//using Marquesita.Infrastructure.ViewModels.Dashboards.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
