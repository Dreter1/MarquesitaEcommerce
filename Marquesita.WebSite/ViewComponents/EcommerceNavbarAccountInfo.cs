using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.WebSite.ViewComponents
{
    public class EcommerceNavbarAccountInfo : ViewComponent
    {
        private readonly IUserManagerService _userManager;

        public EcommerceNavbarAccountInfo(IUserManagerService userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if(User.Identity.Name != null)
            {
                var user = await _userManager.GetUserByNameAsync(User.Identity.Name);
                return View(user);
            }
            return View(new User{ Id = null });

        }
    }
}
