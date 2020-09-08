using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Models.Business;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

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

        public void CreateProduct(Product product, IFormFile image, string path)
        {
            var imagen = UploadedServerFile(path, image);
            product.ImageRoute = imagen;
            product.IsActive = true;
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

        private string UploadedServerFile(string path, IFormFile image)
        {
            string uniqueFileName = null;
            if (image != null)
            {
                string uploadsFolder = Path.Combine(path, "Images", "Products");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private void DeleteServerFile(string path, string image)
        {
            if (image != null)
            {
                string serverFilePath = Path.Combine(path, "Images", "Products", image);

                if (File.Exists(serverFilePath))
                    File.Delete(serverFilePath);
            }
        }
    }
}
