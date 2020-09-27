﻿using Marquesita.Infrastructure.DbContexts;
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

        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<SaleDetailTemp> _saleDetailTempRepository;
        private readonly IRepository<SaleDetail> _saleDetailRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IProductService _productService;
        private readonly BusinessDbContext _context;

        public SaleService(IRepository<Sale> Salerepository,
            IRepository<SaleDetailTemp> SaleDetailTempRepository,
            IRepository<SaleDetail> saleDetailRepository,
            IRepository<Product> productRepository,
            IProductService productService,
            BusinessDbContext context) {
            _saleRepository = Salerepository;
            _saleDetailTempRepository = SaleDetailTempRepository;
            _saleDetailRepository = saleDetailRepository;
            _productRepository = productRepository;
            _productService = productService;
            _context = context;
        }
        public async Task<Sale> GetOrdersAsync(int id)
        {
            return await _context.Sales.FindAsync(id);
        }

        public IEnumerable<Sale> GetSaleList()
        {
            return _saleRepository.All();
        }

        public IEnumerable<SaleDetailTemp> GetClientSaleTempList(string userId)
        {
            var clientSaleDetailtTemp = _context.SaleDetailsTemp.Where(x => x.UserId == userId).ToList();
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

        public void SaveSale(User user, Sale sale, IEnumerable<SaleDetailTemp> saledetailtemp)
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
            _saleRepository.Add(order);
            _saleRepository.SaveChanges();

            foreach (var detail in saledetailtemp)
            {
                Product productoBd = _productService.GetProductById(detail.ProductId);
                SaleDetail detalle = new SaleDetail
                {
                    ProductId = productoBd.Id,
                    SaleId = order.Id,
                    UnitPrice = productoBd.UnitPrice,
                    Quantity = detail.Quantity,
                    Subtotal = productoBd.UnitPrice * detail.Quantity,
                };

                sale.TotalAmount += detalle.Subtotal;
                _saleDetailRepository.Add(detalle);

                RemoveSaleDetailsTemp(detail.ProductId, sale.UserId);
                _saleDetailRepository.SaveChanges();
            }
        }

        public void RemoveSaleDetailsTemp(Guid IdProducto, string userId)
        {
            var saleDetailTemp = _context.SaleDetailsTemp.Where(x => x.ProductId == IdProducto && x.UserId == userId).FirstOrDefault();
            if (saleDetailTemp != null)
            {
                _saleDetailTempRepository.Remove(saleDetailTemp);
                _saleDetailTempRepository.SaveChanges();
            }
        }

        public bool IsProductStocked(AddItemViewModel product)
        {
            var productoBd = _productService.GetProductById(product.Productid);
            if (productoBd.Stock < product.Quantity)
                return false;
            return true;
        }

        public bool StockAvailable(IEnumerable<SaleDetailTemp> productos)
        {
            Product productoBd;

            foreach (var producto in productos)
            {
                productoBd = _productService.GetProductById(producto.ProductId);
                if (productoBd.Stock < producto.Quantity)
                    return false;
            }
            return true;
        }

        public void UpdateStock(IEnumerable<SaleDetailTemp> saledetailTemp)
        {
            Product productoBd;
            foreach (var saledetailT in saledetailTemp)
            {
                productoBd = _context.Products.Where(o => o.Id == saledetailT.ProductId).FirstOrDefault();
                productoBd.Stock -= saledetailT.Quantity;
                _productRepository.Update(productoBd);
                _productRepository.SaveChanges();
            }
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
    }
}

