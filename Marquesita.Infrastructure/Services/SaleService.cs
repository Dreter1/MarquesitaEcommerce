using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Sales;
using Marquesita.Models.Business;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe.Issuing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class SaleService : ISaleService
    {

        private readonly IRepository<Sale> _Salerepository;
        private readonly IProductService _productService;
        private readonly IRepository<SaleDetailTemp> _SaleDetailTempRepository;
        private readonly BusinessDbContext _context;
        private readonly IUserManagerService _userManagerService;

        public SaleService(IRepository<Sale> Salerepository, IRepository<SaleDetailTemp> SaleDetailTempRepository, BusinessDbContext context, IUserManagerService userManagerService, IProductService productService)
        {

            _Salerepository = Salerepository;
            _SaleDetailTempRepository = SaleDetailTempRepository;
            _context = context;
            _userManagerService = userManagerService;
            _productService = productService;

        }

        public async Task<IQueryable<Sale>> GetOrdersAsync(string userName)
        {
            var user = await _userManagerService.GetUserIdByNameAsync(userName);
            if (user == null)
            {
                return null;
            }

            return _context.Sales
                .Include(o => o.SaleDetails)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.Date);
        }
        public async Task<Sale> GetOrdersAsync(int id)
        {
            return await _context.Sales.FindAsync(id);
        }
        public IEnumerable<Sale> GetSaleList()
        {
            return _Salerepository.All();
        }

        public IQueryable<SaleDetailTemp> GetDetailTempsAsync()
        {
          
            return _context.SaleDetailsTemp
                .Include(o => o.Product)
                .OrderBy(o => o.Product.Name);
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {

            var user = await _userManagerService.GetUserIdByNameAsync(userName);
            if (user == null)
            {
                return;
            }
            var product = await _context.Products.FindAsync(model.Productid);
            if (product == null)
            {
                return;
            }
           
            var orderDetailTemp = await _context.SaleDetailsTemp
               .Where(odt => odt.Product == product)
                .FirstOrDefaultAsync();
           
            if (orderDetailTemp == null)
            {
                orderDetailTemp = new SaleDetailTemp
                {
                    Price = product.UnitPrice,
                    Quantity = model.Quantity,
                    ProductId = model.Productid,
                   //Subtotal = (Decimal) product.UnitPrice * model.Quantity,
                    UserId = Guid.Parse(user),
                };

                
                //var disminuir = await _context.Products.Where(o => o.Id == model.Productid).FirstAsync();
                //if (disminuir.Stock >= orderDetailTemp.Quantity)
                //{
                //    disminuir.Stock = disminuir.Stock - orderDetailTemp.Quantity;
                //    _context.SaleDetailsTemp.Add(orderDetailTemp);
                //    await _context.SaveChangesAsync();
                //}
                //else
                //{
                //    //"El stock esta agotado";
                //    return;
                //}
                _context.SaleDetailsTemp.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                _context.SaleDetailsTemp.Update(orderDetailTemp);
            }
           

            await _context.SaveChangesAsync();
        }
       
        public async Task ModifyOrderDetailTempQuantityAsync(Guid id, int quantity)
        {
            var orderDetailTemp = await _context.SaleDetailsTemp.FindAsync(id);
           

            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                _context.SaleDetailsTemp.Update(orderDetailTemp);
                await _context.SaveChangesAsync();
            }

           
        }

        public async Task DeleteDetailTempAsync(Guid id)
        {
            var orderDetailTemp = await _context.SaleDetailsTemp.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            _context.SaleDetailsTemp.Remove(orderDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await _userManagerService.GetUserIdByNameAsync(userName);
            var employee = await _userManagerService.GetUserIdByNameAsync(userName);
           
            if (user == null)
            {
                return false;
            }

            var orderTmps = await _context.SaleDetailsTemp
                .Include(o => o.Product)/*.Include(o => o.UserId)*/
                .Where(o => o.UserId.ToString() == user)
                .ToListAsync();
            //var TotalSale = await _Salerepository.

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new SaleDetail
            {
                UnitPrice = o.Price,
                ProductId = o.ProductId,
                Quantity = o.Quantity,
                Subtotal=o.Subtotal

            }).ToList();

            var order = new Sale
            {
                Date = DateTime.UtcNow,
                UserId = Guid.Parse(user), //guarda usuario
                EmployeeId= Guid.Parse(employee),
                //TotalAmount = ,
                SaleDetails = details,
            };

            _context.Sales.Add(order);
            _context.SaleDetailsTemp.RemoveRange(orderTmps);
            await _context.SaveChangesAsync();

            
            return true;
        }

        public async Task UpdateStockAsync(Guid id) {

            var orderDetailTemp = await _context.SaleDetails.FindAsync(id);
            var disminuir = await _context.Products.Where(o => o.Id == orderDetailTemp.ProductId).FirstAsync();
            if (disminuir.Stock >= orderDetailTemp.Quantity)
            {
                disminuir.Stock = disminuir.Stock - orderDetailTemp.Quantity;
                _context.SaleDetails.Add(orderDetailTemp);
                await _context.SaveChangesAsync();
            }
            else
            {
                //"El stock esta agotado";
                return;
            }
        }
    }
}

