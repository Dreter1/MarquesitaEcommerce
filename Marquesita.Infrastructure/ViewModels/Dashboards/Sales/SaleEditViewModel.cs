using Marquesita.Models.Business;
using System;
using System.ComponentModel;

namespace Marquesita.Infrastructure.ViewModels.Dashboards.Sales
{
    public class SaleEditViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Estado")]
        public string SaleStatus { get; set; }


        public static implicit operator Sale(SaleEditViewModel obj)
        {
            return new Sale
            {
                Id = obj.Id,
                SaleStatus = obj.SaleStatus,
            };
        }

    }
}
