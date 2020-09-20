using System;
using System.Collections.Generic;

namespace Marquesita.Models.Business
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string Conutry { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public Guid UserId { get; set; }

        public List<Sale> Sales { get; set; }
    }
}
