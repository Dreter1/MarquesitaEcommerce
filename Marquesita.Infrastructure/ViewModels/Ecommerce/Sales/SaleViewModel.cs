using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Sales
{
    public class SaleViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Tipo de Pago")]
        public byte PaymentType { get; set; }

        [DisplayName("Tipo de Venta")]
        public byte TypeOfSale { get; set; }

        [DisplayName("Estado")]
        public byte SaleStatus { get; set; }

        [DisplayName("Cliente")]
        public Guid UserId { get; set; }

        [DisplayName("Empleado")]
        public Guid? EmployeeId { get; set; }

        [DisplayName("Dirección")]
        public Guid? AddressId { get; set; }

        public DateTime Date { get; set; }

        public List<SaleDetail> SaleDetails { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Lines { get { return this.SaleDetails == null ? 0 : this.SaleDetails.Count(); } }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get { return this.SaleDetails == null ? 0 : this.SaleDetails.Sum(i => i.Quantity); } }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value { get { return this.SaleDetails == null ? 0 : this.SaleDetails.Sum(i => i.UnitPrice); } }

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


        public static implicit operator Sale(SaleViewModel obj)
        {
            return new Sale
            {
                Id = obj.Id,
                PaymentType = obj.PaymentType,
                TypeOfSale = obj.TypeOfSale,
                SaleStatus = obj.SaleStatus,
                UserId = obj.UserId,
                EmployeeId = obj.EmployeeId,
                AddressId = obj.AddressId
            };
        }

    }
}
