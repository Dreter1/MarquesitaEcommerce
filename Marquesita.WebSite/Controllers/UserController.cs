using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Infrastructure.ViewModels.Dashboards.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        [Authorize(Policy = "CanViewUsers")]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                ViewBag.Image = _images.RoutePathRootEmployeeImages();
                ViewBag.UserId = user.Id;
                return View(await _usersManager.GetUsersEmployeeList());
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        [Authorize(Policy = "CanAddUsers")]
        public async Task<IActionResult> CreateAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
                ViewBag.UserId = user.Id;
                return View();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanAddUsers")]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
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
                ViewBag.UserId = user.Id;
                return View();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        [Authorize(Policy = "CanEditUsers")]
        public async Task<IActionResult> EditAsync(string Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var userEdit = _usersManager.UserToViewModel(await _usersManager.GetUserByIdAsync(Id));

                if (userEdit != null)
                {
                    ViewBag.Image = _images.RoutePathRootEmployeeImages();
                    ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
                    ViewBag.UserId = user.Id;
                    ViewBag.UserRole = await _usersManager.GetUserRole(userEdit);
                    ViewBag.User = userEdit.Id;
                    return View(userEdit);
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanEditUsers")]
        public async Task<IActionResult> Edit(UserEditViewModel model, string Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var userEdit = await _usersManager.GetUserByIdAsync(Id);
                var path = _webHostEnvironment.WebRootPath;

                if (ModelState.IsValid)
                {
                    if (user != null)
                    {
                        _usersManager.UpdatingUser(model, userEdit, model.ProfileImage, path);
                        await _usersManager.UpdatingRoleOfUserAsync(userEdit, model.Role);

                        return RedirectToAction("Index", "User");
                    }
                }

                ViewBag.Image = _images.RoutePathRootEmployeeImages();
                ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
                ViewBag.UserId = user.Id;
                ViewBag.UserRole = userRole;
                ViewBag.User = userEdit.Id;

                return View(model);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanDeleteUsers")]
        public async Task<IActionResult> RemoveRestoreCredentials(string Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var userRemove = await _usersManager.GetUserByIdAsync(Id);
                if (userRemove != null)
                {
                    _usersManager.RemovingRestoringCredentials(userRemove);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        public async Task<IActionResult> ProfileAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var userProfile = await _usersManager.GetUserByNameAsync(User.Identity.Name);

                if (userProfile != null)
                {
                    ViewBag.Image = _images.RoutePathRootEmployeeImages();
                    ViewBag.UserRole = await _usersManager.GetUserRole(userProfile);
                    ViewBag.UserId = userProfile.Id;
                    return View(_usersManager.UserToViewModel(await _usersManager.GetUserByNameAsync(User.Identity.Name)));
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var actualId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);

                if (actualId == Id)
                {
                    var userEditProfile = _usersManager.UserToViewModel(await _usersManager.GetUserByIdAsync(Id));

                    if (userEditProfile != null)
                    {
                        ViewBag.Image = _images.RoutePathRootEmployeeImages();
                        ViewBag.UserId = actualId;
                        ViewBag.User = userEditProfile.Id;
                        return View(userEditProfile);
                    }
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditViewModel model, string Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var userProfile = await _usersManager.GetUserByIdAsync(Id);
                var path = _webHostEnvironment.WebRootPath;

                if (ModelState.IsValid)
                {
                    if (userProfile != null)
                    {
                        _usersManager.UpdatingUser(model, userProfile, model.ProfileImage, path);
                        return RedirectToAction("Profile", "User");
                    }
                }

                ViewBag.Image = _images.RoutePathRootEmployeeImages();
                ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                ViewBag.UserRole = await _usersManager.GetUserRole(userProfile);
                ViewBag.User = userProfile.Id;

                return View(model);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var userPassword = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                if (userPassword != null)
                {
                    var token = await _usersManager.NewTokenPassword(userPassword);
                    var model = new ResetEmployeePassword { Token = token, Email = userPassword.Email };
                    return View(model);
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetEmployeePassword resetPasswordModel)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                if (ModelState.IsValid)
                {
                    var userPassword = await _usersManager.GetUserByEmailAsync(resetPasswordModel.Email);

                    if (userPassword != null)
                    {
                        var resetPassResult = await _usersManager.ChangeEmployeePassword(userPassword, resetPasswordModel);

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
                    return RedirectToAction("NotFound404", "Error");
                }
                return View(resetPasswordModel);
            }
            return RedirectToAction("NotFound404", "Error");
        }
    }
}
