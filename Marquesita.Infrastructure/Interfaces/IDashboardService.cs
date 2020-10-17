using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IDashboardService
    {
        void ProductWiseSales(out string MobileCountList, out string ProductList);
    }
}
