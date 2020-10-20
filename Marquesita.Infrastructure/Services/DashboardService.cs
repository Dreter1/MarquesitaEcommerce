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

            var saleData = new List<decimal>();

            for(int month = 1; month <= 12; month++)
            {
                var monthSale = _context.Sales.Where(sale => sale.Date.Month == month).Sum(total => total.TotalAmount);
                saleData.Add(monthSale);
            }

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
