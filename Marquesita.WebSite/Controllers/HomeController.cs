using Marquesita.Infrastructure.Email;
using Marquesita.Infrastructure.Interfaces;
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
        private readonly IEmailSenderService _emailSender;

        public HomeController(IUserManagerService usersManager, IAuthManagerService signsInManager, IEmailSenderService emailSender)
        {
            _usersManager = usersManager;
            _signsInManager = signsInManager;
            _emailSender = emailSender;
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
                    return View();
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
                    var userRole = await _usersManager.GetUserRole(user);
                    if (userRole == "Cliente")
                    {
                        var signInResult = await _signsInManager.LoginAsync(model.Username, model.Password);
                        if (signInResult.Succeeded)
                        {
                            return LocalRedirect(returnUrl);
                        }
                    }
                    else
                    {
                        ViewBag.LoginError = "Cuenta no valida, por favor revise sus credenciales.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.LoginError = "Usuario o Contraseña Incorrecta";
                    return View();
                }
            }
            ViewBag.LoginError = "Usuario o Contraseña Incorrecta";
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
                    return View();
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
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Home", new { token, email = model.Email }, Request.Scheme);
                    var message = new Message(new string[] { model.Email }, "Confirma tu correo para La Marquesita", user,confirmationLink, null);
                    await _emailSender.SendEmailConfirmationAsync(message);
                    return RedirectToAction("SuccessRegistration", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _usersManager.GetUserByEmailAsync(email);
            if (user == null)
                return RedirectToAction("NotFound404", "Error");
            var result = await _usersManager.ConfirmEmail(user, token);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }
        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
