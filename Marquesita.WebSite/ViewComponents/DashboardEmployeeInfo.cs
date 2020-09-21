using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.WebSite.ViewComponents
{
    public class DashboardEmployeeInfo : ViewComponent
    {
        private readonly IUserManagerService _userManager;

        public DashboardEmployeeInfo(IUserManagerService userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId.ToString());
            return View(new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }
    

    }
}
