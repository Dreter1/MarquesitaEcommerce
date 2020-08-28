using Marquesita.Infrastructure.DbContexts;
using Marquesita.Models.Business;

namespace Marquesita.Infrastructure.Repositories
{
    public class AddressRepositroy : GenericRepository<Address>
    {
        public AddressRepositroy(BusinessDbContext context) : base(context)
        {

        }
    }
}
