using Marquesita.Infrastructure.DbContexts;
using Marquesita.Models.Business;

namespace Marquesita.Infrastructure.Repositories
{
    public class SaleDetailRepository : GenericRepository<SaleDetail>
    {
        public SaleDetailRepository(BusinessDbContext context) : base(context)
        {

        }
    }
}
