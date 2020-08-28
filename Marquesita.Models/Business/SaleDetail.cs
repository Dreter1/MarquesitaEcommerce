using System;
using System.Collections.Generic;
using System.Text;

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

        public Sale sale { get; set; }
        public Product product { get; set; }
    }
}
