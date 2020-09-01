using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserManagerService _usersManager;
        private readonly IRoleManagerService _rolesManager;
        private readonly IConstantService _images;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IUserManagerService usersManager, IRoleManagerService rolesManager, IConstantService images, IWebHostEnvironment webHostEnvironment)
        {
            _usersManager = usersManager;
            _rolesManager = rolesManager;
            _images = images;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> ProfileAsync()
        {
            var userId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            var user = await _usersManager.GetUserByIdAsync(userId);

            if (user != null)
            {
                ViewBag.UserRole = await _usersManager.GetUserRole(user);
                ViewBag.UserId = userId;
                return View(_usersManager.UserToViewModel(await _usersManager.GetUserByNameAsync(User.Identity.Name)));

            }
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string Id)
        {
            var actualId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);

            if (actualId == Id)
            {
                var user = _usersManager.UserToViewModel(await _usersManager.GetUserByIdAsync(Id));

                if (user != null)
                {
                    ViewBag.UserId = actualId;
                    ViewBag.User = user.Id;
                    return View(user);
                }
            }
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditViewModel model, string Id)
        {
            var user = await _usersManager.GetUserByIdAsync(Id);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    _usersManager.UpdatingUser(model, user);
                    return RedirectToAction("Profile", "User");
                }
            }

            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            ViewBag.UserRole = await _usersManager.GetUserRole(user);
            ViewBag.User = user.Id;

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "CanViewUsers")]
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.Image = _images.RoutePathEmployeeImages();
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View(await _usersManager.GetUsersEmployeeList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddUsers")]
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddUsers")]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = _webHostEnvironment.WebRootPath;
                var result = await _usersManager.CreateUserAsync(model, model.Password, model.ProfileImage, path);
                if (result.Succeeded)
                {
                    await _usersManager.AddingRoleToUserAsync(model.Username, model.Role);
                    return RedirectToAction("Index", "User");
                }
            }
            ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
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
                ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
                ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                ViewBag.UserRole = await _usersManager.GetUserRole(user);
                ViewBag.User = user.Id;
                return View(user);
            }
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpPost]
        [Authorize(Policy = "CanEditUsers")]
        public async Task<IActionResult> Edit(UserEditViewModel model, string Id)
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

            ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
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
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var token = await _usersManager.NewTokenPassword(user);
                var model = new ResetEmployeePassword { Token = token, Email = user.Email };
                return View(model);
            }
            return RedirectToAction("NotFound404", "Auth");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetEmployeePassword resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersManager.GetUserByEmailAsync(resetPasswordModel.Email);

                if (user != null)
                {
                    var resetPassResult = await _usersManager.ChangeEmployeePassword(user, resetPasswordModel);

                    if (!resetPassResult.Succeeded)
                    {
                        foreach (var error in resetPassResult.Errors)
                        {
                            ModelState.TryAddModelError(error.Code, error.Description);
                        }
                        return View(resetPasswordModel);
                    }
                    return RedirectToAction("Profile", "User");
                }
                return RedirectToAction("NotFound404", "Auth");
            }
            return View(resetPasswordModel);
        }
    }
}
