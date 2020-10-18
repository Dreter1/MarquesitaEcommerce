using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Marquesita.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly BusinessDbContext _context;
        private readonly AuthIdentityDbContext _authContext;
        private readonly int actualDay;
        private readonly int actualMonth;
        private readonly int actualYear;

        public DashboardService(BusinessDbContext context, AuthIdentityDbContext authContext)
        {
            _context = context;
            _authContext = authContext;
            actualDay = DateTime.Now.Day;
            actualMonth = DateTime.Now.Month;
            actualYear = DateTime.Now.Year;
        }

        public void GetSaleAmountOfMonths(out string saleMonthList)
        {
            var januarySale = _context.Sales.Where(sale => sale.Date.Month == 1).Sum(total => total.TotalAmount);
            var februarySale = _context.Sales.Where(sale => sale.Date.Month == 2).Sum(total => total.TotalAmount);
            var marchSale = _context.Sales.Where(sale => sale.Date.Month == 3).Sum(total => total.TotalAmount);
            var aprilSale = _context.Sales.Where(sale => sale.Date.Month == 4).Sum(total => total.TotalAmount);
            var maySale = _context.Sales.Where(sale => sale.Date.Month == 5).Sum(total => total.TotalAmount);
            var juneSale = _context.Sales.Where(sale => sale.Date.Month == 6).Sum(total => total.TotalAmount);
            var julySale = _context.Sales.Where(sale => sale.Date.Month == 7).Sum(total => total.TotalAmount);
            var augustSale = _context.Sales.Where(sale => sale.Date.Month == 8).Sum(total => total.TotalAmount);
            var septemberSale = _context.Sales.Where(sale => sale.Date.Month == 9).Sum(total => total.TotalAmount);
            var octoberSale = _context.Sales.Where(sale => sale.Date.Month == 10).Sum(total => total.TotalAmount);
            var novemberSale = _context.Sales.Where(sale => sale.Date.Month == 11).Sum(total => total.TotalAmount);
            var decemberSale = _context.Sales.Where(sale => sale.Date.Month == 12).Sum(total => total.TotalAmount);

            var saleData = new List<decimal>
            {
                januarySale,
                februarySale,
                marchSale,
                aprilSale,
                maySale,
                juneSale,
                julySale,
                augustSale,
                septemberSale,
                octoberSale,
                novemberSale,
                decemberSale
            };

            saleMonthList = string.Join(",", saleData);
        }

        public int GetTotalSalesOfDay()
        {
            var totalDaySale = _context.Sales.Where(sale => sale.Date.Day == actualDay && sale.Date.Month == actualMonth && sale.Date.Year == actualYear).ToList().Count();
            return totalDaySale;
        }

        public decimal GetSaleAmountOfDay()
        {
            var daySale = _context.Sales.Where(sale => sale.Date.Day == actualDay && sale.Date.Month == actualMonth && sale.Date.Year == actualYear).Sum(sale => sale.TotalAmount);
            return daySale;
        }

        public int GetNewUsersOfDay()
        {
            var newUsersCount = _authContext.Users.Where(user => user.RegisterDate.Day == actualDay && user.RegisterDate.Month == actualMonth && user.RegisterDate.Year == actualYear).ToList().Count();
            return newUsersCount;
        }

        public IEnumerable<Sale> GetLastestSales()
        {
            var lastestSales = _context.Sales.Where(sale => sale.Date.Day == actualDay && sale.Date.Month == actualMonth && sale.Date.Year == actualYear)
                .OrderBy(sale => sale.Date)
                .ToList()
                .Take(10);
            return lastestSales;
        }
    }
}
