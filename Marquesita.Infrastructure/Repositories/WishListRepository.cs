using Marquesita.Infrastructure.DbContexts;
using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.Repositories
{
    public class WishListRepository : GenericRepository<WishList>
    {
        public WishListRepository(BusinessDbContext context) : base(context)
        {

        }
    }
}
