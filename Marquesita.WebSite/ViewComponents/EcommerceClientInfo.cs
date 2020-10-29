using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Marquesita.WebSite.ViewComponents
{
    public class EcommerceClientInfo : ViewComponent
    {
        private readonly IUserManagerService _userManager;

        public EcommerceClientInfo(IUserManagerService userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {

            var user = await _userManager.GetUserByIdAsync(userId);
            if(user != null)
            {
                return View(new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }
            return View(new User
            {
                FirstName = "",
                LastName = ""
            });
        }
    }
}