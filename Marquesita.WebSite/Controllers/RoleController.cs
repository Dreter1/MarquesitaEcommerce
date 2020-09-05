using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Roles;
using Microsoft.AspNetCore.Authorization;
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
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                ViewBag.UserId = user.Id;
                return View(_rolesManager.GetEmployeeRolesList());
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        [Authorize(Policy = "CanAddRoles")]
        public async Task<IActionResult> CreateAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                ViewBag.Permissions = _rolesManager.PermissionList();
                ViewBag.UserId = user.Id;
                return View();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanAddRoles")]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {

                if (ModelState.IsValid)
                {
                    var result = await _rolesManager.CreateRoleAsync(model);

                    if (result.Succeeded)
                        await _rolesManager.AssignPermissionsToRole(model);

                    return RedirectToAction("Index");
                }

                ViewBag.Permissions = _rolesManager.PermissionList();
                ViewBag.UserId = user.Id;
                return View();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        [Authorize(Policy = "CanViewRoles")]
        public async Task<IActionResult> DetailAsync(string Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var role = await _rolesManager.GetRoleByIdAsync(Id);

                if (role != null)
                {
                    ViewBag.RoleId = role.Id;
                    ViewBag.RolePermissions = _rolesManager.PermissionListOfRole(role);
                    ViewBag.UserId = user.Id;
                    return View(role);
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        [Authorize(Policy = "CanEditRoles")]
        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var role = await _rolesManager.GetRoleByIdAsync(Id);

                if (role != null)
                {
                    if (role.Name != "Super Admin")
                    {
                        ViewBag.RoleId = role.Id;
                        ViewBag.UserId = user.Id;
                        return View(role);
                    }
                    return RedirectToAction("NotFound404", "Error");
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanEditRoles")]
        public async Task<IActionResult> Edit(RoleEditViewModel model, string Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
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
                        return RedirectToAction("NotFound404", "Error");
                    }
                }
                ViewBag.UserId = user.Id;
                ViewBag.RoleId = Id;
                return View(role);
            }
            return RedirectToAction("NotFound404", "Error");
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
