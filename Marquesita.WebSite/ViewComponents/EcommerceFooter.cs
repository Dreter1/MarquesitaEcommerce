using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.WebSite.ViewComponents
{
    public class EcommerceFooter : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
