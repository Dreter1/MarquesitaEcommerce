using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
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
