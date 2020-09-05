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
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                return View(_categoryService.GetCategoryList());
            }
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpGet]
        [Authorize(Policy = "CanAddCategory")]
        public async Task<IActionResult> CreateAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                return View();
            }
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpPost]
        [Authorize(Policy = "CanAddCategory")]
        public async Task<IActionResult> CreateAsync(CategoryViewModel model)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                if (ModelState.IsValid)
                {
                    _categoryService.CreateCategory(model);
                    return RedirectToAction("Index");
                }
                ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                return View();
            }
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpGet]
        [Authorize(Policy = "CanEditCategory")]
        public async Task<IActionResult> EditAsync(Guid Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
            {
                var category = _categoryService.GetCategoryById(Id);

                if (category != null)
                {
                    ViewBag.Id = category.Id;
                    ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
                    return View(category);
                }
                return RedirectToAction("NotFound404", "Auth");
            }
            return RedirectToAction("NotFound404", "Auth");
        }

        [HttpPost]
        [Authorize(Policy = "CanEditCategory")]
        public async Task<IActionResult> EditAsync(CategoryViewModel model, Guid Id)
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var userRole = await _usersManager.GetUserRole(user);

            if (_usersManager.isColaborator(userRole))
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
                ViewBag.Id = Id;
                return View(category);
            }
            return RedirectToAction("NotFound404", "Auth");
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
