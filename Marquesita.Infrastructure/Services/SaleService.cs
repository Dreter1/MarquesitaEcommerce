using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Business;

namespace Marquesita.Infrastructure.Services
{
    public class SaleService
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IRepository<Sale> _SaleRepository;
        private readonly IRepository<SaleDetail> _SaleDetailRepository;
        private readonly IRepository<Product> _ProductRepository;
    }
}
