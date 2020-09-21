using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Business;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Stripe.Issuing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class SaleService : ISaleService
    {
        private readonly BusinessDbContext _context;
        private readonly IRepository<Sale> _Salerepository;
        private readonly IUserManagerService _userManagerService;
        private readonly IRoleManagerService _roleManagerService;

        public SaleService(IUserManagerService userManagerService , BusinessDbContext context, IRoleManagerService roleManagerService, IRepository<Sale> Salerepository)
        {
            _context = context;
            _userManagerService = userManagerService;
            _Salerepository = Salerepository;
            _roleManagerService = roleManagerService;
        }

        public IEnumerable<Sale> GetSaleList()
        {
            return _Salerepository.All();
        }

        public async Task<List<Sale>> GetOrdersAsync(string userName)
        {
            var user = await _userManagerService.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            var userRole = await _userManagerService.GetUserRole(user);
            var role = await _roleManagerService.GetRoleByName(userRole);
            var rolePermissions = _roleManagerService.GetPermissionListOfRoleByRole(role);
            bool hasPermissions = false;

            foreach (var permission in rolePermissions) {
                if (permission.ToString() == "Ver Ventas" || permission.ToString() == "Agregar Venta" || permission.ToString() == "Editar Venta") {
                    hasPermissions = true;
                }
            }

            if (hasPermissions)
            {
                return _context.Sales
                    .Include(o => o.SaleDetails)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.Date)
                    .ToList();
            }
            return null;
        }


    }
}

