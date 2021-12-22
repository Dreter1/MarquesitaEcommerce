using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Marquesita.WebSite.Controllers
{
    [Authorize]
    public class WishListController : Controller
    {

        private readonly IUserManagerService _usersManager;
        private readonly IProductService _productService;
        private readonly IWishListService _wishListService;

        public WishListController(IUserManagerService usersManager, IProductService productService, IWishListService wishListService)
        {
            _usersManager = usersManager;
            _productService = productService;
            _wishListService = wishListService;
        }
        [HttpGet]
        [Authorize(Policy = "Client")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> GetUserWishListAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
            ViewBag.User = user;
            return PartialView(_wishListService.GetUserWishList(user.Id));
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<bool> ProductAndUserExistInWishListAsync(Guid idProduct)
        {
            var userId = (await _usersManager.GetUserByNameAsync(User.Identity.Name)).Id;

            if (idProduct == null && userId == null)
                return false;

            var existInWishList = _wishListService.DoesUserAndProductExistInWishList(idProduct, userId);

            if (!existInWishList)
                return true;

            return false;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<bool> AddProductToWishListAsync(Guid idProduct)
        {
            var userId = (await _usersManager.GetUserByNameAsync(User.Identity.Name)).Id;
            var product = _productService.GetProductById(idProduct);

            if (userId == null || product == null)
                return false;

            _wishListService.CreateWishListItem(idProduct, userId);
            return true;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<Boolean> DeleteItem(Guid id)
        {
            if (id != null)
            {
                await _wishListService.DeleteWishListItem(id);
                return true;
            }
            return false;
        }
    }
}
