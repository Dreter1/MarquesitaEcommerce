using Marquesita.Models.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Marquesita.Models.Business
{
    public class SaleDetailTemp
	{
		public Guid Id { get; set; }

		public decimal Price { get; set; }

		public int Quantity { get; set; }

		//public decimal Subtotal { get; set; }
		//public byte PaymentType { get; set; }
		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }
		
		public Product Product { get; set; }

		public decimal Subtotal { get { return this.Price * (decimal)this.Quantity; } }
	}

}
