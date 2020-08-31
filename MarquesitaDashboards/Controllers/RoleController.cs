using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IUserManagerService _usersManager;
        private readonly IRoleManagerService _rolesManager;

        public RoleController(IUserManagerService usersManager, IRoleManagerService rolesManager)
        {
            _usersManager = usersManager;
            _rolesManager = rolesManager;
        }

        [HttpGet]
        [Authorize(Policy = "CanViewRoles")]
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View(_rolesManager.GetEmployeeRolesList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddRoles")]
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddRoles")]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _rolesManager.CreateRoleAsync(model);

                if (result.Succeeded)
                    await _rolesManager.AssignPermissionsToRole(model);

                return RedirectToAction("Index");
            }

            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }
    }
}
