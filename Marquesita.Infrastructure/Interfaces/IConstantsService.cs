using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IConstantsService
    {
        List<string> PermissionList();
        List<string> GetPaymentList();
        List<string> GetSaleStatusList();
        List<string> GetEcommercePaymentList();
    }
}
