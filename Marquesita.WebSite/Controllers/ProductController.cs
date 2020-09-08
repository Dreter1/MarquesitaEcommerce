using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUserManagerService _usersManager;
        private readonly IProductService _productService;

        public ProductController(IUserManagerService usersManager, IProductService productService)
        {
            _usersManager = usersManager;
            _productService = productService;
        }

        [AllowAnonymous]
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
                    return View(_productService.GetProductList());
                }
                return RedirectToAction("NotFound404", "Error");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanViewProducts")]
        public async Task<IActionResult> ListAsync()
        {
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View(_productService.GetProductList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddProducts")]
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddProducts")]
        public async Task<IActionResult> CreateAsync(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                _productService.CreateProduct(model);
                return RedirectToAction("Index");
            }
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }
    }
}
