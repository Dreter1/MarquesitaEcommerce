using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAuthManagerService _signsInManager;
        private readonly IUserManagerService _usersManager;

        
        public DashboardController(IAuthManagerService signsInManager, IUserManagerService usersManager)
        {
            _signsInManager = signsInManager;
            _usersManager = usersManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl)
        {
            returnUrl ??= Url.Content("/Dashboard/Main");
            if (ModelState.IsValid)
            {
                var user = await _usersManager.GetUserByNameAsync(model.Username);

                if (user != null)
                {
                    var userRole = await _usersManager.GetUserRole(user);
                    if(userRole != ConstantsService.UserType.CLIENT)
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
                            ModelState.AddModelError(string.Empty, ConstantsService.Errors.USER_INACTIVE);
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ConstantsService.Errors.INVALID_ACCOUNT);
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ConstantsService.Errors.INVALID_USER);
                    return View();
                }

            }
            ModelState.AddModelError(string.Empty, ConstantsService.Errors.INVALID_USER);
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult LogOut()
        {
            _signsInManager.LogOut();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MainAsync()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (_usersManager.isColaborator(userRole))
                {
                    ViewBag.UserId = user.Id;
                    return View();
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return RedirectToAction("NotFound404", "Error");
        }
    }
}
