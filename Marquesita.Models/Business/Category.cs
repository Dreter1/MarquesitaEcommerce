using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Models.Business
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Product> products { get; set; }
    }
}
