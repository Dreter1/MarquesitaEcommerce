using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IUserManagerService _usersManager;
        private readonly ICategoryService _categoryService;

        public CategoryController(IUserManagerService usersManager, ICategoryService categoryService)
        {
            _usersManager = usersManager;
            _categoryService = categoryService;
        }

        [Authorize(Policy = "CanViewCategory")]
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View(_categoryService.GetCategoryList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddCategory")]
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddCategory")]
        public async Task<IActionResult> CreateAsync(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                _categoryService.CreateCategory(model);
                return RedirectToAction("Index");
            }
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanEditCategory")]
        public async Task<IActionResult> EditAsync(Guid Id)
        {
            var category = _categoryService.GetCategoryById(Id);

            if (category != null)
            {
                ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                return View(category);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanEditCategory")]
        public async Task<IActionResult> EditAsync(CategoryViewModel model, Guid Id)
        {
            var category = _categoryService.GetCategoryById(Id);

            if (ModelState.IsValid)
            {
                if (category != null)
                {
                    _categoryService.UpdateCategory(model, category);
                    return RedirectToAction("Index");
                }
            }

            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View(category);
        }

        [HttpPost]
        [Authorize(Policy = "CanDeleteCategory")]
        public Boolean Delete(Guid Id)
        {
            var category = _categoryService.GetCategoryById(Id);

            if (category != null)
            {
                _categoryService.DeleteCategory(category);
                return true;
            }
            return false;
        }
    }
}
