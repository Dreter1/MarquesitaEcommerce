using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Models.Business;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProductList();
        IEnumerable<Product> GetProductListByCategory(Guid categoryId);
        Product GetProductById(Guid Id);
        void CreateProduct(Product product, IFormFile image, string path);
        void DeleteProduct(Product product, string path);
        void UpdateProduct(ProductEditViewModel model, Product product, IFormFile image, string path);
        void DeleteServerFile(string path, string image);
        ProductEditViewModel ProductToViewModel(Product obj);
    }
}
