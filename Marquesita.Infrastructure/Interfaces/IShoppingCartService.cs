using Marquesita.Models.Business;
using System;
using System.Collections.Generic;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IShoppingCartService
    {
        IEnumerable<ShoppingCart> getUserCartAsList(string userId);
        bool DoesUserAndProductExistInCart(Guid idProduct, string userId);
        void CreateShoppingCartItem(Guid idProduct, string userId);
    }
}
