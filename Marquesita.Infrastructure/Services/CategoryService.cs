using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Category;
using Marquesita.Models.Business;
using System;
using System.Collections.Generic;

namespace Marquesita.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Categories> _repository;

        public CategoryService(IRepository<Categories> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Categories> GetCategoryList()
        {
            return _repository.All();
        }

        public Categories GetCategoryById(Guid Id)
        {
            return _repository.Get(Id);
        }

        public void CreateCategory(Categories category)
        {
            _repository.Add(category);
            _repository.SaveChanges();
        }

        public void DeleteCategory(Categories category)
        {
            _repository.Remove(category);
        }

        public void UpdateCategory(CategoryViewModel model, Categories category)
        {
            category.Name = model.Name;
            _repository.Update(category);
            _repository.SaveChanges();
        }
    }
}
