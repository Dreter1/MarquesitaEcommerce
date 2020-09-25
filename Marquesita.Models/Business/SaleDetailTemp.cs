using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
		//public User User { get; set; }
		
		//public IEnumerable<SelectListItem> Users { get; set; }
		public decimal Subtotal { get { return this.Price * (decimal)this.Quantity; } }
	}

}
