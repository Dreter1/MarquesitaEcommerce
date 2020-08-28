using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Models.Business
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }

        public Product products { get; set; }
    }
}
