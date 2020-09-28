using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marquesita.Infrastructure.ViewModels.Dashboards.Sales
{
    public class AddItemViewModel
    {
        [DisplayName("Producto")]
        public Guid Productid { get; set; }

        [DisplayName("Cantidad")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "La cantidad no puede ser negativa")]
        public int Quantity { get; set; }

        public string UserId { get; set; }

    }
}
