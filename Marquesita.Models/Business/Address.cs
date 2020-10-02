using System;
using System.Collections.Generic;

namespace Marquesita.Models.Business
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string FullNames { get; set; }
        public string Phone { get; set; }
        public string UserId { get; set; }

        public List<Sale> Sales { get; set; }
    }
}
