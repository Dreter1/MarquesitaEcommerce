using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
            ViewBag.Permissions = _rolesManager.PermissionList();
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

            ViewBag.Permissions = _rolesManager.PermissionList();
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanViewRoles")]
        public async Task<IActionResult> DetailAsync(string Id)
        {
            var role = await _rolesManager.GetRoleByIdAsync(Id);

            if (role != null)
            {
                ViewBag.RoleId = role.Id;
                ViewBag.RolePermissions = _rolesManager.PermissionListOfRole(role);
                ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                return View(role);
            }
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpGet]
        [Authorize(Policy = "CanEditRoles")]
        public async Task<IActionResult> Edit(string Id)
        {
            var role = await _rolesManager.GetRoleByIdAsync(Id);

            if (role != null)
            {
                if (role.Name != "Super Admin")
                {
                    ViewBag.RoleId = role.Id;
                    ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                    return View(role);
                }
                return RedirectToAction("NotFound404", "Auth");
            }
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpPost]
        [Authorize(Policy = "CanEditRoles")]
        public async Task<IActionResult> Edit(RoleEditViewModel model, string Id)
        {
            var role = await _rolesManager.GetRoleByIdAsync(Id);

            if (ModelState.IsValid)
            {
                if (role != null)
                {
                    if (role.Name != "Super Admin")
                    {
                        _rolesManager.UpdateRoles(model, role);
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("NotFound404", "Auth");
                }
            }
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            ViewBag.RoleId = Id;
            return View(role);
        }

        [HttpPost]
        [Authorize(Policy = "CanDeleteRoles")]
        public async Task<Boolean> Delete(string Id)
        {
            var role = await _rolesManager.GetRoleByIdAsync(Id);
            if (role != null)
            {
                if(role.Name != "Super Admin")
                {
                    await _rolesManager.DeletingRoleAsync(role);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
