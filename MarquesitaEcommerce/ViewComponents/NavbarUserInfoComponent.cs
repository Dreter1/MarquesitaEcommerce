using Marquesita.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaEcommerce.ViewComponents
{
    public class NavbarUserInfoComponent : ViewComponent
    {
        private readonly UserManagerService _userManager;

        public NavbarUserInfoComponent(UserManagerService userManager)
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
