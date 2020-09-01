using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthManagerService _signsInManager;
        private readonly IUserManagerService _usersManager;

        public AuthController(IAuthManagerService signsInManager, IUserManagerService usersManager)
        {
            _signsInManager = signsInManager;
            _usersManager = usersManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginViewModel model, string returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _usersManager.GetUserByNameAsync(model.Username);

                if (user != null)
                {
                    var userRole = await _usersManager.GetUserRole(user);
                    if(userRole != "Cliente")
                    {
                        if (user.IsActive)
                        {
                            var signInResult = await _signsInManager.LoginAsync(model.Username, model.Password);
                            if (signInResult.Succeeded)
                            {
                                return LocalRedirect(returnUrl);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Su cuenta fue desactiva, comuniquese con un administrador");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Cuenta no valida, comuniquese con un administrador");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario o Contraseña Incorrecta");
                    return View();
                }

            }
            ModelState.AddModelError(string.Empty, "Usuario o Contraseña Incorrecta");
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult LogOut(string Id)
        {
            _signsInManager.LogOut();
            return RedirectToAction("SignIn");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult NotFound404()
        {
            return View();
        }
    }
}
