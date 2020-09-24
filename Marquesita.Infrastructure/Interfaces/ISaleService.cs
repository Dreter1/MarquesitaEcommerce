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
        Task<IQueryable<Sale>> GetOrdersAsync(string userName);
        Task<Sale> GetOrdersAsync(int id);
        IQueryable<SaleDetailTemp> GetDetailTempsAsync();
        Task AddItemToOrderAsync(AddItemViewModel model, string userName);
        Task ModifyOrderDetailTempQuantityAsync(Guid id, int quantity);
        Task DeleteDetailTempAsync(Guid id);
        Task<bool> ConfirmOrderAsync(string userName);
        Task UpdateStockAsync(Guid id);
    }
}
