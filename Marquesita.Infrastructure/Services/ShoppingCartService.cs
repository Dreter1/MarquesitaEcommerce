using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly BusinessDbContext _context;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, BusinessDbContext context)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _context = context;
        }

        public IEnumerable<ShoppingCart> getUserCartAsList(string userId)
        {
            return _context.ShoppingCarts.Where(cart => cart.UserId == userId).Include(p => p.Products).ToList();
        }

        public bool DoesUserAndProductExistInCart(Guid idProduct, string userId)
        {
            var dbShoppingCart = getUserCartAsList(userId);
            foreach (var cart in dbShoppingCart)
            {
                if (cart.ProductId == idProduct)
                    return true;
            }
            return false;
        }

        public void CreateShoppingCartItem(Guid idProduct, string userId)
        {
            ShoppingCart cartItem = new ShoppingCart
            {
                ProductId = idProduct,
                UserId = userId,
                Quantity = 1
            };

            _shoppingCartRepository.Add(cartItem);
            _shoppingCartRepository.SaveChanges();
        }

        public async Task UpdateQuantityShoppingCartItem(Guid id, int quantity)
        {
            var shoppingCartItem = await _context.ShoppingCarts.FindAsync(id);

            if (shoppingCartItem != null)
            {
                shoppingCartItem.Quantity += quantity;
                if (shoppingCartItem.Quantity > 0)
                {
                    _shoppingCartRepository.Update(shoppingCartItem);
                    _shoppingCartRepository.SaveChanges();
                }
            }
            return;
        }

        public async Task DeleteShoppingCartItem(Guid id)
        {
            var shoppingCartItem = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCartItem != null)
            {
                _shoppingCartRepository.Remove(shoppingCartItem);
                _shoppingCartRepository.SaveChanges();
            }
            return;
        }

        public void RemoveShoppingCartItemForSale(Guid IdProducto, string userId)
        {
            var shoppingCartItem = _context.ShoppingCarts.Where(x => x.ProductId == IdProducto && x.UserId == userId).FirstOrDefault();
            if (shoppingCartItem != null)
            {
                _shoppingCartRepository.Remove(shoppingCartItem);
                _shoppingCartRepository.SaveChanges();
            }
        }
    }
}
