using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Models.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProductList();
        Product GetProductById(Guid Id);
        void CreateProduct(Product product, IFormFile image, string path);
        void DeleteProduct(Product product);
        void UpdateProduct(ProductEditViewModel model, Product product, IFormFile image, string path);
        ProductEditViewModel ProductToViewModel(Product obj);

        IEnumerable<SelectListItem> GetComboProducts();
    }
}
