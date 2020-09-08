using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Models.Business;
using System;
using System.Collections.Generic;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProductList();
        Product GetProductById(Guid Id);
        void CreateProduct(Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(ProductViewModel model, Product Product);
    }
}
