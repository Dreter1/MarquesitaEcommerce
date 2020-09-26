using System;

namespace Marquesita.Models.Business
{
    public class WishList
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string UserId { get; set; }

        public Product Product { get; set; }
    }
}
