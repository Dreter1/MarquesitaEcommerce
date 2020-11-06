using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;
using Marquesita.Infrastructure.ViewModels.Dashboards.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserManagerService _usersManager;
        private readonly IProductService _productService;

        public CategoryController(ICategoryService categoryService, IWebHostEnvironment webHostEnvironment, IUserManagerService usersManager, IProductService productService)
        {
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
            _usersManager = usersManager;
            _productService = productService;
        }

        [Authorize(Policy = "CanViewCategory")]
        public IActionResult Index()
        {
            return View(_categoryService.GetCategoryList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddCategory")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddCategory")]
        public IActionResult Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                _categoryService.CreateCategory(model);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanEditCategory")]
        public IActionResult Edit(Guid Id)
        {
            var category = _categoryService.GetCategoryById(Id);

            if (category != null)
            {
                return View(category);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanEditCategory")]
        public IActionResult Edit(CategoryViewModel model)
        {
            var category = _categoryService.GetCategoryById(model.Id);

            if (ModelState.IsValid)
            {
                if (category != null)
                {
                    _categoryService.UpdateCategory(model, category);
                    return RedirectToAction("Index");
                }
            }

            return View(category);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Detail(Guid categoryId)
        {
            var categorie = _categoryService.GetCategoryById(categoryId);

            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole) && categorie != null)
                {
                    ViewBag.User = user;
                    ViewBag.CategoryId = categoryId;
                    ViewBag.Categorys = _categoryService.GetCategoryList();
                    return View();
                }
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.CategoryId = categoryId;
            ViewBag.Categorys = _categoryService.GetCategoryList();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryProductList(Guid categoryId)
        {
            if (User.Identity.Name != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var userRole = await _usersManager.GetUserRole(user);

                if (!_usersManager.isColaborator(userRole))
                {
                    ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
                    ViewBag.UserId = user.Id;
                    return PartialView(_productService.GetProductListByCategory(categoryId));
                }
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
            return PartialView(_productService.GetProductListByCategory(categoryId));
        }

        [HttpPost]
        [Authorize(Policy = "CanDeleteCategory")]
        public Boolean Delete(Guid Id)
        {
            var category = _categoryService.GetCategoryById(Id);
            var path = _webHostEnvironment.WebRootPath;

            if (category != null)
            {
                _categoryService.DeleteCategory(category, path);
                return true;
            }
            return false;
        }
    }
}
