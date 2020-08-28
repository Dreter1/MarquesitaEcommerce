using Marquesita.Infrastructure.DbContexts;
using Marquesita.Models.Business;

namespace Marquesita.Infrastructure.Repositories
{
    public class SaleRepository : GenericRepository<Sale>
    {
        public SaleRepository(BusinessDbContext context) : base(context)
        {

        }
    }
}
