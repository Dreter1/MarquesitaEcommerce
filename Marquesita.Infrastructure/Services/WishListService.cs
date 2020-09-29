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
    public class WishListService : IWishListService
    {
        private readonly IRepository<WishList> _wishListRepository;
        private readonly BusinessDbContext _context;

        public WishListService(IRepository<WishList> wishListRepository, BusinessDbContext context)
        {
            _wishListRepository = wishListRepository;
            _context = context;
        }

        public IEnumerable<WishList> GetUserWishList(string userId)
        {
            return _context.WishLists.Where(wish => wish.UserId == userId).Include(p => p.Product).ToList();

        }

        public bool DoesUserAndProductExistInWishList(Guid idProduct, string userId)
        {
            var dbWishList = GetUserWishList(userId);
            foreach (var wishList in dbWishList)
            {
                if (wishList.ProductId == idProduct)
                    return true;
            }
            return false;
        }

        public void CreateWishListItem(Guid idProduct, string userId)
        {
            WishList wishListItem = new WishList
            {
                ProductId = idProduct,
                UserId = userId
            };

            _wishListRepository.Add(wishListItem);
            _wishListRepository.SaveChanges();
        }

        public async Task DeleteWishListItem(Guid id)
        {
            var wishListItem = await _context.WishLists.FindAsync(id);
            if (wishListItem != null)
            {
                _wishListRepository.Remove(wishListItem);
                _wishListRepository.SaveChanges();
            }
            return;
        }
    }
}
