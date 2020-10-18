using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IDashboardService
    {
        void GetSaleAmountOfMonths(out string saleMonthList);
        int GetTotalSalesOfDay();
        decimal GetSaleAmountOfDay();
        int GetNewUsersOfDay();
        IEnumerable<Sale> GetLastestSales();
    }
}
