using Marquesita.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Marquesita.WebSite.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUserManagerService _usersManager;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IConstantService _images;

        public ShoppingCartController(IUserManagerService usersManager, IProductService productService, IShoppingCartService shoppingCartService, IConstantService images)
        {
            _usersManager = usersManager;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _images = images;
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> GetUserCartItemsAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            ViewBag.Image = _images.RoutePathRootProductsImages();
            return PartialView(_shoppingCartService.getUserCartAsList(user.Id));
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public bool ProductAndUserExistInCart(Guid idProduct, string userId)
        {
            if (idProduct == null && userId == null)
                return false;

            var existInCart = _shoppingCartService.DoesUserAndProductExistInCart(idProduct, userId);

            if (!existInCart)
                return true;

            return false;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<bool> AddProductToCartAsync(Guid idProduct, string userId)
        {
            var user = await _usersManager.GetUserByIdAsync(userId);
            var product = _productService.GetProductById(idProduct);

            if (user == null || product == null)
                return false;

            _shoppingCartService.CreateShoppingCartItem(idProduct, userId);
            return true;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<Boolean> Increase(Guid id)
        {
            if (id != null)
            {
                await _shoppingCartService.UpdateQuantityShoppingCartItem(id, 1);
                return true;
            }
            return false;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<Boolean> Decrease(Guid id)
        {
            if (id != null)
            {
                await _shoppingCartService.UpdateQuantityShoppingCartItem(id, -1);
                return true;
            }
            return false;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<Boolean> DeleteItem(Guid id)
        {
            if (id != null)
            {
                await _shoppingCartService.DeleteShoppingCartItem(id);
                return true;
            }
            return false;
        }
    }
}
