using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce;
//using Marquesita.Infrastructure.ViewModels.Dashboards.Users;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    public class ClientController : Controller
    {

        public IActionResult MyProfile()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
