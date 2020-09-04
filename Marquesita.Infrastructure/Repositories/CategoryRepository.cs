using Marquesita.Infrastructure.DbContexts;
using Marquesita.Models.Business;

namespace Marquesita.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Categories>
    {
        public CategoryRepository(BusinessDbContext context) : base(context)
        {

        }
    }
}
