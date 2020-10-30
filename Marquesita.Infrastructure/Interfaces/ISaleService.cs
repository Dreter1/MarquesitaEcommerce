using Marquesita.Infrastructure.ViewModels.Dashboards.Sales;
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
        Sale GetSaleById(Guid id);
        IEnumerable<Sale> GetSaleList();
        int GetSaleListCount();
        IEnumerable<Sale> GetClientSaleList(string userId);
        IEnumerable<SaleDetail> GetDetailSaleList(Guid saleId);
        IEnumerable<SaleDetailTemp> GetClientSaleTempList(string userId);
        Task<Sale> GetOrdersAsync(int id);
        Task AddItemToClientOrderSaleAsync(AddItemViewModel model);
        Task ModifyOrderDetailTempQuantityAsync(Guid id, int quantity);
        Task DeleteDetailTempAsync(Guid id);
        Sale SaveSale(User user, Sale sale, IEnumerable<SaleDetailTemp> saledetailtemp);
        void RemoveSaleDetailsTemp(Guid IdProducto, string userId);
        bool IsProductStocked(Guid productId, int quantity);
        bool StockAvailable(IEnumerable<SaleDetailTemp> productos);
        void UpdateStock(IEnumerable<SaleDetailTemp> saledetailTemp);
        Sale SaveEcommerceSale(Sale sale, IEnumerable<ShoppingCart> shoppigCart);
        bool StockAvailableEcommerce(IEnumerable<ShoppingCart> products);
        void UpdateStockEcommerce(IEnumerable<ShoppingCart> shoppingCartItems);
        bool IsGreaterThan0(int quantity);
        bool SaleExists(Guid saleId);
        bool IsUserSale(Guid saleId, string userId);
    }
}
