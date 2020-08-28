using System;
using System.Collections.Generic;

namespace Marquesita.Models.Business
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public byte TipeOfPay { get; set; }
        public byte SaleStatus { get; set; }

        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        
        public List<SaleDetail> saleDetails { get; set; }
        public Address address { get; set; }
    }
}
