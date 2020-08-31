using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserManagerService _usersManager;
        private readonly IRoleManagerService _rolesManager;

        public UserController(IUserManagerService usersManager, IRoleManagerService rolesManager)
        {
            _usersManager = usersManager;
            _rolesManager = rolesManager;
        }

        [HttpGet]
        [Authorize(Policy = "CanViewUsers")]
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View(_usersManager.GetUsersList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddUsers")]
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.Roles = _rolesManager.GetRolesList();
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddUsers")]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _usersManager.CreateUserAsync(model, model.Password);
                if (result.Succeeded)
                {
                    await _usersManager.AddingRoleToUserAsync(model.Username, model.Role);
                    return RedirectToAction("Index", "User");
                }
            }
            ViewBag.Roles = _rolesManager.GetRolesList();
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanEditUsers")]
        public async Task<IActionResult> EditAsync(string Id)
        {
            var user = _usersManager.UserToViewModel(await _usersManager.GetUserByIdAsync(Id));

            if (user != null)
            {
                ViewBag.Roles = _rolesManager.GetRolesList();
                ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                ViewBag.UserRole = await _usersManager.GetUserRole(user);
                ViewBag.User = user.Id;
                return View(user);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        [Authorize(Policy = "CanEditUsers")]
        public async Task<IActionResult> Edit(UserViewModel model, string Id)
        {
            var user = await _usersManager.GetUserByIdAsync(Id);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    _usersManager.UpdatingUser(model, user);
                    await _usersManager.UpdatingRoleOfUserAsync(user, model.Role);

                    return RedirectToAction("Index", "User");
                }
            }

            ViewBag.Roles = _rolesManager.GetRolesList();
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            ViewBag.UserRole = await _usersManager.GetUserRole(user);
            ViewBag.User = user.Id;

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "CanDeleteUsers")]
        public async Task<IActionResult> RemoveRestoreCredentials(string Id)
        {
            var user = await _usersManager.GetUserByIdAsync(Id);
            if (user != null)
            {
                _usersManager.RemovingRestoringCredentials(user);
                return RedirectToAction("Index");
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }
    }
}
