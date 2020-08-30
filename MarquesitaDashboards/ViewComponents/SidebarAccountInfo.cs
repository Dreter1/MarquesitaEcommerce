using Marquesita.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.ViewComponents
{
    public class SidebarAccountInfo : ViewComponent
    {
        private readonly IUserManagerService _userManager;

        public SidebarAccountInfo(IUserManagerService userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserByNameAsync(User.Identity.Name);
            return View(user);
        }
    }
}
