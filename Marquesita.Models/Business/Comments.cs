using System;

namespace Marquesita.Models.Business
{
    public class Comments
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
