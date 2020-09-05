using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Marquesita.WebSite.Controllers
{
    public class WishListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
