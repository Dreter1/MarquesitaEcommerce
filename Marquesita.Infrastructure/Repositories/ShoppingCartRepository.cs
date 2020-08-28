using Marquesita.Infrastructure.DbContexts;
using Marquesita.Models.Business;

namespace Marquesita.Infrastructure.Repositories
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>
    {
        public ShoppingCartRepository(BusinessDbContext context) : base(context)
        {

        }
    }
}
