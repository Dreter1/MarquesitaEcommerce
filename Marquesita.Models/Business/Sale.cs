using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Marquesita.Models.Business
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public byte PaymentType { get; set; }
        public byte TypeOfSale { get; set; }
        public byte SaleStatus { get; set; }

        public Guid UserId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? AddressId { get; set; }

        public List<SaleDetail> SaleDetails { get; set; }
        public Address Address { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? Lines { get { return this.SaleDetails == null ? 0 : this.SaleDetails.Count(); } }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int Quantity { get { return this.SaleDetails == null ? 0 : this.SaleDetails.Sum(i => i.Quantity); } }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value { get { return SaleDetails == null ? 0 : SaleDetails.Sum(i => i.UnitPrice); } }

        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? OrderDateLocal
        {
            get
            {
                if (this.Date == null)
                {
                    return null;
                }

                return this.Date.ToLocalTime();
            }
        }

    }
}
