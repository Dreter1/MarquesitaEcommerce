using Marquesita.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.ViewComponents
{
    public class SidebarAccountInfo : ViewComponent
    {
        private readonly IUserManagerService _userManager;
        private readonly IConstantService _images;

        public SidebarAccountInfo(IUserManagerService userManager, IConstantService images)
        {
            _userManager = userManager;
            _images = images;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Image = _images.RoutePathRootEmployeeImages();
            var user = await _userManager.GetUserByNameAsync(User.Identity.Name);
            return View(user);
        }
    }
}
