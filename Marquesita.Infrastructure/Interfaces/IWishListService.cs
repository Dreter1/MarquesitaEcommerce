using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IWishListService
    {
        IEnumerable<WishList> GetUserWishList(string userId);
        bool DoesUserAndProductExistInWishList(Guid idProduct, string userId);
        void CreateWishListItem(Guid idProduct, string userId);
        Task DeleteWishListItem(Guid id);
    }
}
