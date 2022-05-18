using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marquesita.WebSite.ViewComponents
{
    public class EcommerceFooter : ViewComponent
    {
        private readonly IUserManagerService _userManager;

        public EcommerceFooter(IUserManagerService userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.Name != null)
            {
                var user = await _userManager.GetUserByNameAsync(User.Identity.Name);
                return View(user);
            }
            return View(new User { Id = null });
        }
    }
}
