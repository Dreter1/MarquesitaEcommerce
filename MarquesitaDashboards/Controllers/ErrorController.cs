using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marquesita.WebSite.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IUserManagerService _usersManager;

        public ErrorController(IUserManagerService usersManager)
        {
            _usersManager = usersManager;
        }
        [HttpGet]
        public async Task<IActionResult> AccessDeniedAsync()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var role = await _usersManager.GetUserRole(user);

                if (user != null)
                {
                    return View(new RoleViewModel { Name = role });
                }
                return View(new RoleViewModel { Name = null });
            }
            return View(new RoleViewModel { Name = null });

        }
        [HttpGet]
        public async Task<IActionResult> NotFound404Async()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var role = await _usersManager.GetUserRole(user);

                if (user != null)
                {
                    return View(new RoleViewModel { Name = role });
                }
                return View(new RoleViewModel{Name = null});
            }
            return View(new RoleViewModel { Name = null });
        }
    }
}
