using System;

namespace Marquesita.Models.Business
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }

        public Product Products { get; set; }
    }
}
