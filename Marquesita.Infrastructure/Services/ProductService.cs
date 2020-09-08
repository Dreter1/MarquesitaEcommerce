using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Models.Business;
using System;
using System.Collections.Generic;

namespace Marquesita.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Product> GetProductList()
        {
            return _repository.All();
        }

        public Product GetProductById(Guid Id)
        {
            return _repository.Get(Id);
        }

        public void CreateProduct(Product product)
        {
            _repository.Add(product);
            _repository.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            _repository.Remove(product);
        }

        public void UpdateProduct(ProductViewModel model, Product Product)
        {
            Product.Name = model.Name;
            _repository.Update(Product);
            _repository.SaveChanges();
        }
    }
}
