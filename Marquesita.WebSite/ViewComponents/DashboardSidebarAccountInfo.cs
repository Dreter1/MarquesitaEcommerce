using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.ViewComponents
{
    public class DashboardSidebarAccountInfo : ViewComponent
    {
        private readonly IUserManagerService _userManager;

        public DashboardSidebarAccountInfo(IUserManagerService userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_COLABORATOR;
            var user = await _userManager.GetUserByNameAsync(User.Identity.Name);
            return View(user);
        }
    }
}
