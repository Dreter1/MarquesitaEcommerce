using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marquesita.Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _repository;
        private readonly BusinessDbContext _context;

        public ShoppingCartService(IRepository<ShoppingCart> repository, BusinessDbContext context)
        {
            _repository = repository;
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

            _repository.Add(cartItem);
            _repository.SaveChanges();
        }
    }
}
