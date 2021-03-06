﻿using Marquesita.Models.Business;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Products
{
    public class ProductViewModel
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

        [DisplayName("Imagen de producto")]
        public IFormFile ProductImage { get; set; }


        public static implicit operator Product(ProductViewModel obj)
        {
            return new Product
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description,
                Stock = obj.Stock,
                UnitPrice = obj.UnitPrice,
                CategoryId = obj.CategoryId,
                ImageRoute = obj.ImageRoute
            };
        }
    }
}
