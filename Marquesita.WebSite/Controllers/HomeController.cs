using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using MarquesitaDashboards.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserManagerService _usersManager;
        private readonly IAuthManagerService _signsInManager;
        private readonly IMailService _mailService;

        public HomeController(IUserManagerService usersManager, IAuthManagerService signsInManager, IMailService mailService)
        {
            _usersManager = usersManager;
            _signsInManager = signsInManager;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    ViewBag.UserId = user.Id;
                    return View();
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AboutAsync()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    ViewBag.UserId = user.Id;
                    return View();
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ContactAsync()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    ViewBag.UserId = user.Id;
                    return View();
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoginAsync()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            returnUrl ??= Url.Content("/Home/Index");
            if (ModelState.IsValid)
            {
                var user = await _usersManager.GetUserByNameAsync(model.Username);

                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        var userRole = await _usersManager.GetUserRole(user);
                        if (userRole == ConstantsService.UserType.CLIENT)
                        {
                            var signInResult = await _signsInManager.LoginAsync(model.Username, model.Password);
                            if (signInResult.Succeeded)
                            {
                                return LocalRedirect(returnUrl);
                            }
                        }
                        else
                        {
                            ViewBag.LoginError = ConstantsService.Errors.INVALID_USER_TYPE;
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.LoginError = ConstantsService.Errors.VALIDATE_EMAIL;
                        return View();
                    }

                }
                else
                {
                    ViewBag.LoginError = ConstantsService.Errors.INVALID_USER;
                    return View();
                }
            }
            ViewBag.LoginError = ConstantsService.Errors.INVALID_USER;
            return View();
        }

        [Authorize(Roles = "Cliente")]
        [HttpGet]
        public IActionResult LogOut()
        {
            _signsInManager.LogOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> RegisterAsync()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ClientViewModel model)
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
                    await _mailService.GenerateAndSendConfirmationEmail(user, emailConfirmationLink);
                    return RedirectToAction("SuccessRegistration", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }

            var user = await _usersManager.GetUserByEmailAsync(email);
            if (user == null)
                return RedirectToAction("NotFound404", "Error");
            var result = await _usersManager.ConfirmEmail(user, token);

            if (result.Succeeded)
            {
                return View("ConfirmEmail", "Home");
            }
            else {
                return RedirectToAction("NotFound404", "Error");
            }

        }

        [HttpGet]
        public async Task<IActionResult> ResendConfirmationEmail(string token, string email)
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }

            var user = await _usersManager.GetUserByEmailAsync(email);
            var emailConfirmationLink = Url.Action("ConfirmEmail", "Home", new { token , email }, Request.Scheme);
            await _mailService.GenerateAndSendConfirmationEmail(user, emailConfirmationLink);
            return RedirectToAction("SuccessRegistration", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> SuccessRegistrationAsync()
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPasswordAsync()
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }

            if (!ModelState.IsValid)
                return View(forgotPasswordModel);

            var user = await _usersManager.GetUserByEmailAsync(forgotPasswordModel.Email);

            if (user == null)
                return RedirectToAction("ForgotPasswordConfirmation", "Home");

            var token = await _usersManager.NewTokenPassword(user);
            TempData["userEmail"] = user.Email;
            TempData["userToken"] = token;
            var resetPasswordLink = Url.Action("ResetPassword", "Home", new { token, email = user.Email }, Request.Scheme);
            await _mailService.GenerateAndSendResetPassword(user, resetPasswordLink);
            return RedirectToAction("ForgotPasswordConfirmation", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ResendResetPasswordEmail(string token, string email)
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }

            var user = await _usersManager.GetUserByEmailAsync(email);
            var resetPasswordLink = Url.Action("ResetPassword", "Home", new { token, email }, Request.Scheme);
            await _mailService.GenerateAndSendResetPassword(user, resetPasswordLink);
            return RedirectToAction("ForgotPasswordConfirmation", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPasswordConfirmationAsync()
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordAsync(string token, string email)
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }

            if (!ModelState.IsValid)
                return View(resetPasswordModel);
            var user = await _usersManager.GetUserByEmailAsync(resetPasswordModel.Email);

            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation", "Home");

            var resetPassResult = await _usersManager.ChangeClientPassword(user, resetPasswordModel);

            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction("ResetPasswordConfirmation", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ResetPasswordConfirmationAsync()
        {
            if (User.Identity.Name != null)
            {
                var userLog = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(userLog);

                if (!_usersManager.isColaborator(userRole))
                {
                    _signsInManager.LogOut();
                    return View("Index", "Home");
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
