using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Sales
{
    public class AddItemViewModel
    {

        [DisplayName("Producto")]
        public Guid Productid { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantiy must be a positive number")]
        public int Quantity { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }
    }
}
