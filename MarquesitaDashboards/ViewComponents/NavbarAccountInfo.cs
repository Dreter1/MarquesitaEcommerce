using Marquesita.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.ViewComponents
{
    public class NavbarAccountInfo : ViewComponent
    {
        private readonly IUserManagerService _userManager;

        public NavbarAccountInfo(IUserManagerService userManager)
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
