using ClosedXML.Excel;
using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Sales;
using Marquesita.Models.Business;
using Marquesita.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IShoppingCartService _shoppingCartService;
        private readonly BusinessDbContext _context;

        public SaleService(IRepository<Sale> Salerepository,
            IRepository<SaleDetailTemp> SaleDetailTempRepository,
            IRepository<SaleDetail> saleDetailRepository,
            IRepository<Product> productRepository,
            IProductService productService,
            IShoppingCartService shoppingCartService,
            BusinessDbContext context)
        {
            _saleRepository = Salerepository;
            _saleDetailTempRepository = SaleDetailTempRepository;
            _saleDetailRepository = saleDetailRepository;
            _productRepository = productRepository;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _context = context;
        }
        public async Task<Sale> GetOrdersAsync(int id)
        {
            return await _context.Sales.FindAsync(id);
        }

        public Sale GetSaleById(Guid id)
        {
            if (id == null)
                return null;

            return _context.Sales.Where(sale => sale.Id == id).Include(address => address.Address).FirstOrDefault();
        }


        public IEnumerable<Sale> GetSaleList()
        {
            return _saleRepository.All().OrderByDescending(sale => sale.Date);
        }

        public IEnumerable<Sale> GetClientSaleList(string userId)
        {
            if (userId == null)
                return null;

            return _context.Sales.Where(sale => sale.UserId == userId).Include(address => address.Address).ToList();
        }

        public IEnumerable<SaleDetail> GetDetailSaleList(Guid saleId)
        {
            return _context.SaleDetails.Where(saleDetail => saleDetail.SaleId == saleId).Include(products => products.Product).ToList();
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
            if (saledetailtemp.Count() > 0)
            {
                var order = new Sale
                {
                    UserId = sale.UserId,
                    TotalAmount = sale.TotalAmount,
                    EmployeeId = user.Id,
                    Date = DateTime.Now,
                    PaymentType = sale.PaymentType,
                    SaleStatus = ConstantsService.SaleStatus.IN_PROCESS,
                    TypeOfSale = ConstantsService.SaleType.STORE_SALE
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

        public bool IsProductStocked(Guid productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            if (product.Stock < quantity)
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

        public void SaveEcommerceSale(Sale sale, IEnumerable<ShoppingCart> shoppigCart)
        {
            if (shoppigCart.Count() > 0)
            {
                var order = new Sale
                {
                    Date = DateTime.Now,
                    UserId = sale.UserId,
                    TotalAmount = sale.TotalAmount,
                    PaymentType = sale.PaymentType,
                    AddressId = sale.AddressId,
                    SaleStatus = ConstantsService.SaleStatus.IN_PROCESS,
                    TypeOfSale = ConstantsService.SaleType.ECOMMERCE_SALE
                };
                _saleRepository.Add(order);
                _saleRepository.SaveChanges();

                foreach (var detail in shoppigCart)
                {
                    var product = _productService.GetProductById(detail.ProductId);
                    var saleDetail = new SaleDetail
                    {
                        ProductId = product.Id,
                        SaleId = order.Id,
                        UnitPrice = product.UnitPrice,
                        Quantity = detail.Quantity,
                        Subtotal = product.UnitPrice * detail.Quantity,
                    };

                    sale.TotalAmount += saleDetail.Subtotal;
                    _saleDetailRepository.Add(saleDetail);

                     _shoppingCartService.RemoveShoppingCartItemForSale(detail.ProductId, sale.UserId);
                    _saleDetailRepository.SaveChanges();
                }
            }
        }

        public bool StockAvailableEcommerce(IEnumerable<ShoppingCart> products)
        {
            Product product;
            foreach (var shoppingCartItem in products)
            {
                product = _productService.GetProductById(shoppingCartItem.ProductId);
                if (product.Stock < shoppingCartItem.Quantity)
                    return false;
            }
            return true;
        }

        public void UpdateStockEcommerce(IEnumerable<ShoppingCart> shoppingCartItems)
        {
            Product product;
            foreach (var item in shoppingCartItems)
            {
                product = _context.Products.Where(o => o.Id == item.ProductId).FirstOrDefault();
                product.Stock -= item.Quantity;
                _productRepository.Update(product);
                _productRepository.SaveChanges();
            }
        }

        public bool IsGreaterThan0(int quantity)
        {
            if (quantity <= 0)
                return false;
            else
                return true;
        }

        public bool SaleExists(Guid saleId)
        {
            if (saleId != null)
            {
                var sale = GetSaleById(saleId);
                if (sale != null)
                {
                    var saleList = GetSaleList();
                    foreach (var list in saleList)
                    {
                        if (list.Id == saleId)
                            return true;
                    }
                }
            }
            return false;
        }

        public bool IsUserSale(Guid saleId, string userId)
        {
            if (saleId != null && userId != null)
            {
                var sale = GetSaleById(saleId);
                if (sale != null)
                {
                    var saleList = GetSaleList();
                    foreach (var list in saleList)
                    {
                        if (list.Id == saleId && list.UserId == userId)
                            return true;
                    }
                }
            }
            return false;
        }

        public byte[] GenerateExcelReport()
        {
            var sales = GetSaleList();
            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Ventas");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Fecha y hora";
            worksheet.Cell(currentRow, 2).Value = "Metodo de Pago";
            worksheet.Cell(currentRow, 3).Value = "Tipo de venta";
            worksheet.Cell(currentRow, 4).Value = "Estado de venta";
            worksheet.Cell(currentRow, 5).Value = "Monto de venta";

            foreach (var sale in sales)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = sale.Date;
                worksheet.Cell(currentRow, 2).Value = sale.PaymentType;
                worksheet.Cell(currentRow, 3).Value = sale.TypeOfSale;
                worksheet.Cell(currentRow, 4).Value = sale.SaleStatus;
                worksheet.Cell(currentRow, 5).Value = "S/. " + sale.TotalAmount;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return content;
        }
    }
}

