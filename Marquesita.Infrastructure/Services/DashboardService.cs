using Dapper;
using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Sales;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Marquesita.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IConfiguration _configuration;

        public DashboardService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ProductWiseSales(out string MobileCountList, out string ProductList)
        {
            string connectionString = _configuration.GetConnectionString("BusinessDB");
            using SqlConnection con = new SqlConnection(connectionString);

            var saletdata = con.Query<SaleViewModel>("Usp_GetTotalsalesProductwise", null, null, true, 0, CommandType.StoredProcedure).ToList();
            var MobileSalesCounts = (from temp in saletdata
                                     select temp.Quantity).ToList();

            var ProductNames = (from temp in saletdata
                                select temp.Date).ToList();

            MobileCountList = string.Join(",", MobileSalesCounts);

            ProductList = string.Join(",", ProductNames);



        }
    }
}
