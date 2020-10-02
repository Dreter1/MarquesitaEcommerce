using Marquesita.Models.Business;
using System;
using System.ComponentModel;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Clients
{
    public class AddressEditViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Calle")]
        public string Street { get; set; }

        [DisplayName("Region")]
        public string Region { get; set; }

        [DisplayName("Ciudad")]
        public string City { get; set; }

        [DisplayName("Codigo Postal")]
        public string PostalCode { get; set; }

        [DisplayName("Nombres y Apellidos")]
        public string FullNames { get; set; }

        [DisplayName("Celular")]
        public string Phone { get; set; }

        public static implicit operator Address(AddressEditViewModel obj)
        {
            return new Address
            {
                Id = obj.Id,
                Street = obj.Street,
                Region = obj.Region,
                City = obj.City,
                PostalCode = obj.PostalCode,
                FullNames = obj.FullNames,
                Phone = obj.Phone
            };
        }
    }
}
