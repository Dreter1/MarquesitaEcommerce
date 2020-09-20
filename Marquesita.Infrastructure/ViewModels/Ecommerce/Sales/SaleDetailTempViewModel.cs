using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Sales
{
    public class SaleDetailTempViewModel
    {
		public Guid Id { get; set; }

		[DisplayFormat(DataFormatString = "{0:C2}")]
		public decimal Price { get; set; }

		[DisplayFormat(DataFormatString = "{0:N2}")]
		public double Quantity { get; set; }

		[DisplayFormat(DataFormatString = "{0:C2}")]
		public decimal Subtotal { get { return this.Price * (decimal)this.Quantity; } }
	}
}
