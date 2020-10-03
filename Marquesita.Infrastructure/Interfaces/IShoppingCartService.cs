using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IShoppingCartService
    {
        IEnumerable<ShoppingCart> GetUserCartAsList(string userId);
        bool DoesUserAndProductExistInCart(Guid idProduct, string userId);
        void CreateShoppingCartItem(Guid idProduct, string userId);
        Task UpdateQuantityShoppingCartItem(Guid id, int quantity);
        Task DeleteShoppingCartItem(Guid id);
        void RemoveShoppingCartItemForSale(Guid IdProducto, string userId);
    }
}
