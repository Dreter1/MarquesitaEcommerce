using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

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
