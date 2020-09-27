using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Sales;
using Marquesita.Models.Business;
using Marquesita.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class SaleService : ISaleService
    {

        private readonly IRepository<Sale> _Salerepository;
        private readonly IRepository<SaleDetailTemp> _saleDetailTempRepository;
        private readonly IRepository<SaleDetail> _saleDetailRepository;
        private readonly BusinessDbContext _context;
        private readonly IProductService _productService;



        private readonly IUserManagerService _userManagerService;

        public SaleService(IRepository<Sale> Salerepository, IRepository<SaleDetailTemp> SaleDetailTempRepository, BusinessDbContext context, IProductService productService, IRepository<SaleDetail> saleDetailRepository, IUserManagerService userManagerService)
        {

            _Salerepository = Salerepository;
            _saleDetailTempRepository = SaleDetailTempRepository;
            _context = context;
            _productService = productService;
            _saleDetailRepository = saleDetailRepository;
            _userManagerService = userManagerService;
        }
        public async Task<Sale> GetOrdersAsync(int id)
        {
            return await _context.Sales.FindAsync(id);
        }

        public IEnumerable<Sale> GetSaleList()
        {
            return _Salerepository.All();
        }

        public IEnumerable<SaleDetailTemp> GetClientSaleTempList(string userId)
        {
            var id = Guid.Parse(userId);
            var clientSaleDetailtTemp = _context.SaleDetailsTemp.Where(x => x.UserId == id).ToList();
            return clientSaleDetailtTemp;
        }

        public async Task AddItemToClientOrderSaleAsync(AddItemViewModel model)
        {
            var orderDetailTemp = _context.SaleDetailsTemp.Where(x => x.UserId == model.UserId && x.ProductId == model.Productid).FirstOrDefault();
            var product = await _context.Products.FindAsync(model.Productid);

            if (product == null)
            {
                return;
            }

            if (orderDetailTemp == null)
            {
                orderDetailTemp = new SaleDetailTemp
                {
                    Price = product.UnitPrice,
                    Quantity = model.Quantity,
                    ProductId = model.Productid,
                    UserId = model.UserId
                };
                _saleDetailTempRepository.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                _saleDetailTempRepository.Update(orderDetailTemp);
            }
            _saleDetailTempRepository.SaveChanges();
        }

        public async Task ModifyOrderDetailTempQuantityAsync(Guid id, int quantity)
        {
            var orderDetailTemp = await _context.SaleDetailsTemp.FindAsync(id);

            if (orderDetailTemp != null)
            {
                orderDetailTemp.Quantity += quantity;
                if (orderDetailTemp.Quantity > 0)
                {
                    _saleDetailTempRepository.Update(orderDetailTemp);
                    _saleDetailTempRepository.SaveChanges();
                }
            }
            return;
        }

        public async Task DeleteDetailTempAsync(Guid id)
        {
            var orderDetailTemp = await _context.SaleDetailsTemp.FindAsync(id);
            if (orderDetailTemp != null)
            {
                _saleDetailTempRepository.Remove(orderDetailTemp);
                _saleDetailTempRepository.SaveChanges();
            }
            return;
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await _userManagerService.GetUserIdByNameAsync(userName);
            return true;
            //var user = await _userManagerService.GetUserIdByNameAsync(userName);
            //var employee = await _userManagerService.GetUserIdByNameAsync(userName);

            //if (user == null)
            //{
            //    return false;
            //}

            //var orderTmps = await _context.SaleDetailsTemp
            //    .Include(o => o.Product)/*.Include(o => o.UserId)*/
            //    .Where(o => o.UserId.ToString() == user)
            //    .ToListAsync();
            ////var TotalSale = await _Salerepository.

            //if (orderTmps == null || orderTmps.Count == 0)
            //{
            //    return false;
            //}

            ///*
            // Primero la venta

            //var order = new Sale
            //{
            //    Date = DateTime.UtcNow,
            //    UserId = Guid.Parse(user), //guarda usuario
            //    EmployeeId= Guid.Parse(employee),
            //};

            //var orderId = order.Id;

            //var details = orderTmps.Select(o => new SaleDetail
            //{
            //    UnitPrice = o.Price,
            //    ProductId = o.ProductId,
            //    Quantity = o.Quantity,
            //    Subtotal=o.Subtotal

            //}).ToList();


            // */

            //var details = orderTmps.Select(o => new SaleDetail
            //{
            //    UnitPrice = o.Price,
            //    ProductId = o.ProductId,
            //    Quantity = o.Quantity,
            //    Subtotal = o.Subtotal

            //}).ToList();

            //var order = new Sale
            //{
            //    Date = DateTime.UtcNow,
            //    UserId = Guid.Parse(user), //guarda usuario
            //    EmployeeId = Guid.Parse(employee),
            //    //TotalAmount = ,
            //    SaleDetails = details,
            //};

            //_context.Sales.Add(order);
            //_context.SaleDetailsTemp.RemoveRange(orderTmps);
            //await _context.SaveChangesAsync();


            //return true;
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
            //else
            //{
            //    //"El stock esta agotado";
            //    return;
            //}
        }

        public List<string> GetPaymentList()
        {
            return new List<string>() {
                "Efectivo",
                "Tarjeta debito/credito",
                "Efectivo y Tarjeta",
            };
        }

        public List<string> GetSaleStatusList()
        {
            return new List<string>() {
                "En proceso",
                "Confirmada",
                "Cancelada",
            };
        }
        public async Task SaveSale(User user, Sale sale, IEnumerable<SaleDetailTemp> saledetailtemp)
        {
           
            var order = new Sale
            {
                UserId = sale.UserId,
                TotalAmount = sale.TotalAmount,
                EmployeeId = user.Id,
                Date = DateTime.UtcNow,
                PaymentType = sale.PaymentType,
                SaleStatus = "En proceso",
                TypeOfSale = "En Tienda"
                
            };
            _Salerepository.Add(order);
            _Salerepository.SaveChanges();
            foreach (var detail in saledetailtemp)
            {
                Product productoBd = _productService.GetProductById(detail.ProductId);
                SaleDetail detalle = new SaleDetail
                {
                    ProductId = productoBd.Id,
                    SaleId=order.Id,
                    UnitPrice= productoBd.UnitPrice,
                    Quantity = detail.Quantity,
                    Subtotal = productoBd.UnitPrice * detail.Quantity,

                };

                sale.TotalAmount += detalle.Subtotal;
                _saleDetailRepository.Add(detalle);
                
                await RemoveSaleDetailsTemp(detail.ProductId);
                //await StockAvailable(saledetailtemp);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveSaleDetailsTemp(Guid? IdProducto)
        {
            var orderTmps = await _context.SaleDetailsTemp
                    .Include(o => o.Product)
                    .Where(o => o.ProductId == IdProducto )
                    .ToListAsync();

            if (orderTmps != null || orderTmps.Count != 0)
            {
                var details = orderTmps.Select(o => new SaleDetail
                {
                    UnitPrice = o.Price,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    Subtotal = o.Subtotal,
                    

                }).ToList();
                _context.SaleDetailsTemp.RemoveRange(orderTmps);
                await _context.SaveChangesAsync();
            }
            

        }

        public bool StockAvailable(IEnumerable<SaleDetailTemp> productos)
        {
            Product productoBd;

            foreach (var producto in productos)
            {
                productoBd= _productService.GetProductById(producto.ProductId);
                if (productoBd.Stock < producto.Quantity)
                   
                return false;
            }
            return true;
        }

        public async Task UpdateStock(IEnumerable<SaleDetailTemp> saledetailTemp)
        {
            Product productoBd;
            foreach (var saledetailT in saledetailTemp)
            {
                productoBd = await _context.Products.Where(o => o.Id == saledetailT.ProductId).FirstOrDefaultAsync();
                productoBd.Stock = productoBd.Stock - saledetailT.Quantity;
                _context.SaveChanges();
            }
            
        }


    }
}

