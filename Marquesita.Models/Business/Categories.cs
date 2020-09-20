using System;
using System.Collections.Generic;

namespace Marquesita.Models.Business
{
    public class Categories
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
