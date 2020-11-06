using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUserManagerService _usersManager;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUserManagerService usersManager, IProductService productService, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _usersManager = usersManager;
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IndexAsync()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    ViewBag.User = user;
                    ViewBag.Categorys = _categoryService.GetCategoryList();
                    return View();
                }
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.Categorys = _categoryService.GetCategoryList();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductList()
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
                    ViewBag.UserId = user.Id;
                    return PartialView(_productService.GetProductList());
                }
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
            return PartialView(_productService.GetProductList());
        }

        [HttpGet]
        [Authorize(Policy = "CanViewProducts")]
        public IActionResult List()
        {
            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
            return View(_productService.GetProductList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddProducts")]
        public IActionResult Create()
        {
            ViewBag.Categorias = _categoryService.GetCategoryList();
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddProducts")]
        public IActionResult Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = _webHostEnvironment.WebRootPath;
                _productService.CreateProduct(model, model.ProductImage, path);
                return RedirectToAction("List");
            }
            ViewBag.Categorias = _categoryService.GetCategoryList();
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanEditProducts")]
        public IActionResult Edit(Guid Id)
        {
            var product = _productService.ProductToViewModel(_productService.GetProductById(Id));

            if (product != null)
            {
                ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
                ViewBag.Categorias = _categoryService.GetCategoryList();
                return View(product);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanEditProducts")]
        public IActionResult Edit(ProductEditViewModel model)
        {
            var product = _productService.GetProductById(model.Id);
            var path = _webHostEnvironment.WebRootPath;

            if (ModelState.IsValid)
            {
                if (product != null)
                {
                    _productService.UpdateProduct(model, product, model.ProductImage, path);
                    return RedirectToAction("List");
                }
            }

            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
            ViewBag.Categorias = _categoryService.GetCategoryList();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DetailAsync(Guid Id)
        {
            var product = _productService.GetProductById(Id);

            if (product == null)
                return RedirectToAction("NotFound404", "Error");

            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
                    ViewBag.User = user;
                    return View(product);
                }
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
            return View(product);
        }

        [HttpPost]
        [Authorize(Policy = "CanDeleteProducts")]
        public Boolean Delete(Guid Id)
        {
            var product = _productService.GetProductById(Id);
            var path = _webHostEnvironment.WebRootPath;

            if (product != null)
            {
                _productService.DeleteProduct(product, path);
                return true;
            }
            return false;
        }
    }
}
