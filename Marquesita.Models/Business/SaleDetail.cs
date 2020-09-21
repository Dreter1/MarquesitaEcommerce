using System;

namespace Marquesita.Models.Business
{
    public class SaleDetail
    {
        public Guid Id { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }

        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }

        public Sale Sale { get; set; }
        public Product Product { get; set; }

    }
}
