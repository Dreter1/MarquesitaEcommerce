using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marquesita.WebSite.ViewComponents
{
    public class EcommerceNavbarAccountInfo : ViewComponent
    {
        private readonly IUserManagerService _userManager;
        private readonly IProductService _productService;

        public EcommerceNavbarAccountInfo(IUserManagerService userManager, IProductService productService)
        {
            _userManager = userManager;
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.ProductList = _productService.GetProductList();
            if (User.Identity.Name != null)
            {
                var user = await _userManager.GetUserByNameAsync(User.Identity.Name);
                return View(user);
            }
            return View(new User{ Id = null });

        }
    }
}
