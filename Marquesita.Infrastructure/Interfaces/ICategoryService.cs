using Marquesita.Infrastructure.ViewModels.Dashboards.Category;
using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Categories> GetCategoryList();
        Categories GetCategoryById(Guid Id);
        void CreateCategory(Categories category);
        void DeleteCategory(Categories category, string path);
        void UpdateCategory(CategoryViewModel model, Categories category);
    }
}
