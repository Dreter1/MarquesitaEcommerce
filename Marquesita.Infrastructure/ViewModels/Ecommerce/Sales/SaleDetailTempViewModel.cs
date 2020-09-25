using Marquesita.Models.Business;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Sales
{
    public class SaleDetailTempViewModel
    {
		public Guid Id { get; set; }

		[DisplayFormat(DataFormatString = "{0:C2}")]
		public decimal Price { get; set; }

		[DisplayFormat(DataFormatString = "{0:N2}")]
		public int Quantity { get; set; }

		[DisplayFormat(DataFormatString = "{0:C2}")]
		public decimal Subtotal { get { return this.Price * (decimal)this.Quantity; } }
		
		[DisplayName("Cliente")]
		public Guid UserId { get; set; }

		[DisplayName("Producto")]
		public Guid ProductId { get; set; }

		public static implicit operator SaleDetailTemp(SaleDetailTempViewModel obj)
		{
			return new SaleDetailTemp
			{
				Id = obj.Id,
				Price = obj.Price,
				Quantity = obj.Quantity,
				UserId=obj.UserId,
				ProductId=obj.ProductId			
			};
		}

	}

}
