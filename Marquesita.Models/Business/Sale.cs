using System;
using System.Collections.Generic;

namespace Marquesita.Models.Business
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; }
        public string TypeOfSale { get; set; }
        public string SaleStatus { get; set; }

        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public Guid? AddressId { get; set; }

        public List<SaleDetail> SaleDetails { get; set; }
        public Address Address { get; set; }

    }
}
