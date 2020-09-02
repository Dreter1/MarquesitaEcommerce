using Marquesita.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async System.Threading.Tasks.Task<IActionResult> IndexAsync()
        {
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View(_categoryService.GetCategoryList());
        }
    }
}
