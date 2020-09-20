using Marquesita.Infrastructure.DbContexts;
using Marquesita.Models.Business;

namespace Marquesita.Infrastructure.Repositories
{
    public class SaleDetailTempRepository : GenericRepository<SaleDetailTemp>
    {
        public SaleDetailTempRepository(BusinessDbContext context) : base(context)
        {

        }
    }
}
