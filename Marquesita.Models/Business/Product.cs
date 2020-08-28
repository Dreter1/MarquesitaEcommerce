using System;
using System.Collections.Generic;
using System.Text;

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

        public Category Category { get; set; }
        public List<SaleDetail> saleDetails { get; set; }
        public List<FavoriteList> favoriteLists { get; set; }
        public List<ShoppingCart> ShopingCartItems { get; set; }
        public List<Comments> comments { get; set; }
    }
}
