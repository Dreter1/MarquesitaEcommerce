using Marquesita.Infrastructure.ViewModels.Ecommerce.Sales;
using Marquesita.Models.Business;
using Marquesita.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface ISaleService
    {
        IEnumerable<Sale> GetSaleList();
        IEnumerable<SaleDetailTemp> GetClientSaleTempList(string userId);
        Task<Sale> GetOrdersAsync(int id);
        Task AddItemToClientOrderSaleAsync(AddItemViewModel model);
        Task ModifyOrderDetailTempQuantityAsync(Guid id, int quantity);
        Task DeleteDetailTempAsync(Guid id);
        Task<bool> ConfirmOrderAsync(string userName);
        Task UpdateStockAsync(Guid id);
        List<string> GetPaymentList();
        List<string> GetSaleStatusList();


    }
}
