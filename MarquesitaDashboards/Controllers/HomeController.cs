using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MarquesitaDashboards.Models;
using Microsoft.AspNetCore.Authorization;
using Marquesita.Infrastructure.Interfaces;

namespace MarquesitaDashboards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserManagerService _usersManager;

        public HomeController(ILogger<HomeController> logger, IUserManagerService usersManager)
        {
            _logger = logger;
            _usersManager = usersManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            if(User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    ViewBag.UserId = user.Id;
                    return View();
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
