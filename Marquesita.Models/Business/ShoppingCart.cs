using System;

namespace Marquesita.Models.Business
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }
        public string UserId { get; set; }

        public Product Products { get; set; }
    }
}
