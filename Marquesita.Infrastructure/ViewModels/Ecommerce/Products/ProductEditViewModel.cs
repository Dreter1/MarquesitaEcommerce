using Marquesita.Models.Business;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Products
{
    public class ProductEditViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("Descripcion")]
        public string Description { get; set; }
        public string ImageRoute { get; set; }

        [DisplayName("Stock disponible")]
        public int Stock { get; set; }

        [DisplayName("Precio Unitario")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Categoría")]
        public Guid CategoryId { get; set; }

        [DisplayName("Producto Activo")]
        public bool IsActive { get; set; }

        [DisplayName("Imagen de producto")]
        public IFormFile ProductImage { get; set; }


        public static implicit operator Product(ProductEditViewModel obj)
        {
            return new Product
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description,
                Stock = obj.Stock,
                UnitPrice = obj.UnitPrice,
                CategoryId = obj.CategoryId,
                ImageRoute = obj.ImageRoute,
                IsActive = obj.IsActive
            };
        }
    }
}
