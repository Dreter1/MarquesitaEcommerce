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
        private readonly IProductService _productService;

        public CategoryService(IRepository<Categories> repository, IProductService productService)
        {
            _repository = repository;
            _productService = productService;
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

        public void DeleteCategory(Categories category, string path)
        {
            var productList = _productService.GetProductListByCategory(category.Id);
            foreach(var product in productList)
            {
                _productService.DeleteServerFile(path, product.ImageRoute);
            }
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
