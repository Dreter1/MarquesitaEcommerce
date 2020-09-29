using Marquesita.Infrastructure.Interfaces;
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
        private readonly IConstantService _images;

        public WishListController(IUserManagerService usersManager, IProductService productService, IWishListService wishListService, IConstantService images)
        {
            _usersManager = usersManager;
            _productService = productService;
            _wishListService = wishListService;
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
        public async Task<IActionResult> GetUserWishListAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            ViewBag.Image = _images.RoutePathRootProductsImages();
            ViewBag.User = user;
            return PartialView(_wishListService.GetUserWishList(user.Id));
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public bool ProductAndUserExistInWishList(Guid idProduct, string userId)
        {
            if (idProduct == null && userId == null)
                return false;

            var existInWishList = _wishListService.DoesUserAndProductExistInWishList(idProduct, userId);

            if (!existInWishList)
                return true;

            return false;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<bool> AddProductToWishListAsync(Guid idProduct, string userId)
        {
            var user = await _usersManager.GetUserByIdAsync(userId);
            var product = _productService.GetProductById(idProduct);

            if (user == null || product == null)
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
