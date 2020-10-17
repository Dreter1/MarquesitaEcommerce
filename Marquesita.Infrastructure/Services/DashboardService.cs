using Dapper;
using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Sales;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace Marquesita.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly BusinessDbContext _context;
        public DashboardService(BusinessDbContext context)
        {
            _context = context;
        }
        public void ProductWiseSales(out string MobileCountList, out string ProductList)
        {
            
            string connectionString = ConfigurationManager.AppSettings["BusinessDB"];
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               
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
}
