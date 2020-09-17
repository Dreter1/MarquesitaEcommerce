﻿using Marquesita.Infrastructure.Interfaces;
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
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
        public IActionResult Edit(CategoryViewModel model, Guid Id)
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
