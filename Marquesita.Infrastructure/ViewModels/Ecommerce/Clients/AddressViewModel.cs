using Marquesita.Models.Business;
using System;
using System.ComponentModel;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Clients
{
    public class AddressViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Calle")]
        public string Street { get; set; }

        [DisplayName("Pais")]
        public string Country { get; set; }

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

        public string UserId { get; set; }

        public static implicit operator Address(AddressViewModel obj)
        {
            return new Address
            {
                Id = obj.Id,
                Country = obj.Country,
                Street = obj.Street,
                Region = obj.Region,
                City = obj.City,
                PostalCode = obj.PostalCode,
                FullNames = obj.FullNames,
                Phone = obj.Phone,
                UserId = obj.UserId
            };
        }
    }
}
