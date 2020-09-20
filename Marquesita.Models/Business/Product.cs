using System;
using System.Collections.Generic;

namespace Marquesita.Models.Business
{
    public class Product
    {
        public Guid Id { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageRoute { get; set; }
        public int Stock { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsActive { get; set; }
        public Guid CategoryId { get; set; }

        public Categories Category { get; set; }
        public List<SaleDetail> SaleDetails { get; set; }
        public List<SaleDetailTemp> SaleDetailsTemp { get; set; }
        public List<WishList> WishLists { get; set; }
        public List<ShoppingCart> ShopingCartItems { get; set; }
        public List<Comments> Comments { get; set; }
    }
}
