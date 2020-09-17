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
        private readonly IRoleManagerService _rolesManager;

        public RoleController(IRoleManagerService rolesManager)
        {
            _rolesManager = rolesManager;
        }

        [HttpGet]
        [Authorize(Policy = "CanViewRoles")]
        public IActionResult Index()
        {
            return View(_rolesManager.GetEmployeeRolesList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddRoles")]
        public IActionResult Create()
        {
            ViewBag.Permissions = _rolesManager.PermissionList();
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
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanViewRoles")]
        public async Task<IActionResult> DetailAsync(string Id)
        {
            var role = await _rolesManager.GetRoleByIdAsync(Id);

            if (role != null)
            {
                ViewBag.RolePermissions = _rolesManager.PermissionListOfRole(role);
                return View(role);
            }
            return RedirectToAction("NotFound404", "Error");
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
                    return View(role);
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
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
                    return RedirectToAction("NotFound404", "Error");
                }
            }
            return View(role);
        }

        [HttpPost]
        [Authorize(Policy = "CanDeleteRoles")]
        public async Task<Boolean> Delete(string Id)
        {
            var role = await _rolesManager.GetRoleByIdAsync(Id);
            if (role != null)
            {
                if (role.Name != "Super Admin")
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
