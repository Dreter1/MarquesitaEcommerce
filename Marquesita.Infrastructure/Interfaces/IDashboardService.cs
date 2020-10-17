using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IDashboardService
    {
        void GetSaleAmountOfMonths(out string saleMonthList);
    }
}
