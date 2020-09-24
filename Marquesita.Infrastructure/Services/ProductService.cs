using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Models.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Marquesita.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly BusinessDbContext _context;

        public ProductService(IRepository<Product> repository, BusinessDbContext context)
        {
            _repository = repository;
            _context = context;
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

        public void UpdateProduct(ProductEditViewModel model, Product product, IFormFile image, string path)
        {
            product.Name = model.Name;
            product.Description = model.Description;
            product.Stock = model.Stock;
            product.UnitPrice = model.UnitPrice;
            product.CategoryId = model.CategoryId;
            product.IsActive = model.IsActive;

            if (product.ImageRoute != null)
            {
                if (image != null)
                {
                    DeleteServerFile(path, product.ImageRoute);
                    var imagen = UploadedServerFile(path, image);
                    product.ImageRoute = imagen;
                }
                else
                {
                    product.ImageRoute = product.ImageRoute;
                }
            }
            else
            {
                DeleteServerFile(path, product.ImageRoute);
                var imagen = UploadedServerFile(path, image);
                product.ImageRoute = imagen;
            }

            _repository.Update(product);
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

        public ProductEditViewModel ProductToViewModel(Product obj)
        {
            if (obj != null)
            {
                return new ProductEditViewModel
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Description = obj.Description,
                    Stock = obj.Stock,
                    UnitPrice = obj.UnitPrice,
                    ImageRoute = obj.ImageRoute,
                    CategoryId = obj.CategoryId,
                    IsActive = obj.IsActive
                };
            }
            return null;
        }

        public IEnumerable<SelectListItem> GetComboProducts()
        {
            var list = _context.Products.Select(p => new SelectListItem
            {   
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Producto)",
                Value = "0"
            });

            return list;
        }

    }
}
