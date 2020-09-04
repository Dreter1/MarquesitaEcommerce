using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.ViewComponents
{
    public class UserRoleInfo : ViewComponent
    {
        private readonly IUserManagerService _userManager;

        public UserRoleInfo(IUserManagerService userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            var role = await _userManager.GetUserRole(user);
            return View(new RoleViewModel
            {
                Name = role
            });
        }
    }
}
