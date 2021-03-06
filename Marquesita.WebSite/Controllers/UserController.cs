﻿using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Infrastructure.ViewModels.Dashboards.Users;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailService _mailService;

        public UserController(IUserManagerService usersManager, IRoleManagerService rolesManager, IWebHostEnvironment webHostEnvironment, IMailService mailService)
        {
            _usersManager = usersManager;
            _rolesManager = rolesManager;
            _webHostEnvironment = webHostEnvironment;
            _mailService = mailService;
        }

        [AllowAnonymous]
        public bool IsLogged()
        {
            if (User.Identity.Name != null)
                return true;
            return false;
        }

        [HttpGet]
        [Authorize(Policy = "CanViewUsers")]
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_COLABORATOR;
            return View(await _usersManager.GetUsersEmployeeList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddUsers")]
        public IActionResult Create()
        {
            ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
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
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanEditUsers")]
        public async Task<IActionResult> EditAsync(string Id)
        {
            var user = _usersManager.UserToViewModel(await _usersManager.GetUserByIdAsync(Id));

            if (user != null)
            {
                ViewBag.Image = ConstantsService.Images.IMG_ROUTE_COLABORATOR;
                ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
                ViewBag.UserRole = await _usersManager.GetUserRole(user);
                return View(user);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanEditUsers")]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var user = await _usersManager.GetUserByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var path = _webHostEnvironment.WebRootPath;
                    _usersManager.UpdatingUser(model, user, model.ProfileImage, path);
                    await _usersManager.UpdatingRoleOfUserAsync(user, model.Role);
                    return RedirectToAction("Index", "User");
                }
            }

            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_COLABORATOR;
            ViewBag.Roles = _rolesManager.GetEmployeeRolesList();
            ViewBag.UserRole = await _usersManager.GetUserRole(user);

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "CanDeleteUsers")]
        public async Task<bool> RemoveRestoreCredentials(string Id)
        {
            var userRemove = await _usersManager.GetUserByIdAsync(Id);
            if (userRemove != null)
            {
                _usersManager.RemovingRestoringCredentials(userRemove);
                return true;
            }
            return false;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ProfileAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                if (user != null)
                {
                    ViewBag.Image = ConstantsService.Images.IMG_ROUTE_COLABORATOR;
                    ViewBag.UserRole = await _usersManager.GetUserRole(user);
                    return View(_usersManager.UserToViewModel(await _usersManager.GetUserByNameAsync(User.Identity.Name)));
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile(string Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var actualId = user.Id;
                if (actualId == Id)
                {
                    var userEditProfile = _usersManager.UserToViewModel(await _usersManager.GetUserByIdAsync(Id));

                    if (userEditProfile != null)
                    {
                        ViewBag.Image = ConstantsService.Images.IMG_ROUTE_COLABORATOR;
                        return View(userEditProfile);
                    }
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditViewModel model)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                if (ModelState.IsValid)
                {
                    var userProfile = await _usersManager.GetUserByIdAsync(model.Id);         
                    if (userProfile != null)
                    {
                        var path = _webHostEnvironment.WebRootPath;
                        _usersManager.UpdatingUser(model, userProfile, model.ProfileImage, path);
                        return RedirectToAction("Profile", "User");
                    }
                }

                ViewBag.Image = ConstantsService.Images.IMG_ROUTE_COLABORATOR;
                return View(model);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ResetPasswordAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var userPassword = await _usersManager.GetUserByNameAsync(user.UserName);
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

        [Authorize]
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

        [HttpGet]
        [Authorize(Policy = "CanAddUsers")]
        public IActionResult RegisterClient()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddUsers")]
        public async Task<IActionResult> RegisterClient(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _usersManager.CreateClientAsync(model, model.Password);
                if (result.Succeeded)
                {
                    await _usersManager.AddingRoleToClientAsync(model.Username);

                    var user = await _usersManager.GetUserByEmailAsync(model.Email);
                    var token = await _usersManager.ConfirmationEmailToken(user);
                    TempData["userEmail"] = model.Email;
                    TempData["userToken"] = token;
                    var emailConfirmationLink = Url.Action("ConfirmEmail", "Home", new { token, email = model.Email }, Request.Scheme);
                    var forgotPasswordLink = $"{Request.Scheme}://{Request.Host}{Url.Action("ForgotPassword", "Home")}";
                    await _mailService.GenerateAndSendConfirmationEmailByShop(user, emailConfirmationLink, forgotPasswordLink);
                    return RedirectToAction("AddClientSale", "Sale");
                }
            }
            return View();
        }
    }
}
