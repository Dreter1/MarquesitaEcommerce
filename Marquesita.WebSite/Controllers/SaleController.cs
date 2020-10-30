using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;
using Marquesita.Infrastructure.ViewModels.Dashboards.Sales;
using Marquesita.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IProductService _productService;
        private readonly IUserManagerService _usersManager;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAddressService _addressService;
        private readonly IConstantsService _constants;
        private readonly IDocumentsService _documentService;
        private readonly IMailService _mailService;

        public SaleController(ISaleService saleService, IUserManagerService usersManager, IProductService productService, IShoppingCartService shoppingCartService, IAddressService addressService, IConstantsService constants, IDocumentsService documentService, IMailService mailService)
        {
            _saleService = saleService;
            _usersManager = usersManager;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _addressService = addressService;
            _constants = constants;
            _documentService = documentService;
            _mailService = mailService;
        }

        [HttpGet]
        [Authorize(Policy = "CanViewSales")]
        public IActionResult Index()
        {
            ViewBag.SaleSuccess = TempData["saleSuccess"];
            TempData["saleSuccess"] = null;
            return View(_saleService.GetSaleList());
        }

        [HttpGet]
        [Authorize(Policy = "CanViewSales")]
        public IActionResult Detail(Guid saleId)
        {
            if (_saleService.SaleExists(saleId))
            {
                ViewBag.Sale = _saleService.GetSaleById(saleId);
                return View(_saleService.GetDetailSaleList(saleId));
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> AddClientSaleAsync()
        {
            return View(await _usersManager.GetUsersClientsList());
        }

        [HttpGet]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> AddProductClientSaleAsync(string userId)
        {
            var user = await _usersManager.GetUserByIdAsync(userId);

            if (user != null)
            {
                ViewBag.User = user;
                ViewBag.SaleDatailTemp = _saleService.GetClientSaleTempList(user.Id);
                return View();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> GetSaleDetailTempList(string userId)
        {
            var user = await _usersManager.GetUserByIdAsync(userId);

            if (user != null)
            {
                ViewBag.User = user;
                ViewBag.SaleDatailTemp = _saleService.GetClientSaleTempList(user.Id);
                return PartialView();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        [Authorize(Policy = "CanAddSales")]
        public IActionResult AddProductSale(string userId)
        {
            if (userId != null)
            {
                ViewBag.UserId = userId;
                ViewBag.Product = _productService.GetProductList();
                return PartialView();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanAddSales")]
        public async Task<Boolean> AddProductSale(Guid Productid, int Quantity, string UserId)
        {
            TempData["error"] = null;
            TempData["stockError"] = null;
            var model = new AddItemViewModel
            {
                Productid = Productid,
                Quantity = Quantity,
                UserId = UserId
            };
            if (_saleService.IsGreaterThan0(model.Quantity))
            {
                if (_saleService.IsProductStocked(model.Productid, model.Quantity))
                {
                    await _saleService.AddItemToClientOrderSaleAsync(model);
                    return true;
                }
                TempData["stockError"] = ConstantsService.Errors.STOCK_NOT_AVALIBLE;
                return false;
            }
            TempData["error"] = ConstantsService.Errors.INVALID_QUANTITY;
            return false;
        }

        [HttpPost]
        [Authorize(Policy = "CanAddSales")]
        public async Task<Boolean> Increase(Guid id)
        {
            if (id != null)
            {
                await _saleService.ModifyOrderDetailTempQuantityAsync(id, 1);
                return true;
            }
            return false;
        }

        [HttpPost]
        [Authorize(Policy = "CanAddSales")]
        public async Task<Boolean> Decrease(Guid id)
        {
            if (id != null)
            {
                await _saleService.ModifyOrderDetailTempQuantityAsync(id, -1);
                return true;
            }
            return false;
        }

        [HttpPost]
        [Authorize(Policy = "CanAddSales")]
        public async Task<Boolean> DeleteItem(Guid id)
        {
            if (id != null)
            {
                await _saleService.DeleteDetailTempAsync(id);
                return true;
            }
            return false;
        }

        [HttpGet]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> CreateSale(string userId)
        {
            var user = await _usersManager.GetUserByIdAsync(userId);

            if (user != null)
            {
                ViewBag.User = user;
                ViewBag.Product = _productService.GetProductList();
                ViewBag.SaleDatailTemp = _saleService.GetClientSaleTempList(user.Id.ToString());
                ViewBag.PaymentList = _constants.GetPaymentList();
                return View();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanAddSales")]
        public async Task<bool> CreateSaleAsync(string PaymentType, string UserId, decimal TotalAmount)
        {
            TempData["error"] = null;
            TempData["stockError"] = null;
            var sale = new Sale
            {
                TotalAmount = TotalAmount,
                PaymentType = PaymentType,
                UserId = UserId
            };
            var employee = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            IEnumerable<SaleDetailTemp> tempList = _saleService.GetClientSaleTempList(sale.UserId);

            if (tempList.Count() > 0)
            {
                if (_saleService.StockAvailable(tempList))
                {
                    _saleService.UpdateStock(tempList);
                    var saleSuccess = _saleService.SaveSale(employee, sale, tempList);
                    TempData["saleSuccess"] = ConstantsService.Messages.SALE_SUCCESS;
                    await _mailService.GenerateAndSendSaleShopEmail(UserId, saleSuccess);
                    return true;
                }
                TempData["stockError"] = ConstantsService.Errors.STOCK_NOT_AVALIBLE;
                return false;
            }
            TempData["error"] = ConstantsService.Errors.EMPTY_SALE;
            return false;
        }

        [HttpGet]
        [Authorize(Policy = "CanEditSales")]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public IActionResult Checkout()
        {
            ViewBag.stockError = TempData["stockError"];
            ViewBag.error = TempData["error"];
            TempData["stockError"] = null;
            TempData["error"] = null;
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> GetCheckoutAsync()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_PRODUCT;
            ViewBag.ShoppingCart = _shoppingCartService.GetUserCartAsList(user.Id);
            ViewBag.AddressList = _addressService.GetUserAddresses(user.Id);
            ViewBag.PaymentList = _constants.GetEcommercePaymentList();
            return PartialView();
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public async Task<bool> CheckStock()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            IEnumerable<ShoppingCart> shoppingCartList = _shoppingCartService.GetUserCartAsList(user.Id);

            if (_saleService.StockAvailableEcommerce(shoppingCartList))
                return true;

            return false;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<bool> Payment(Guid addressId, string paymentType, decimal TotalAmount)
        {
            var addressIdExists = _addressService.GetAddressById(addressId);
            if(addressIdExists != null)
            {
                var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
                var sale = new Sale
                {
                    TotalAmount = TotalAmount,
                    PaymentType = paymentType,
                    UserId = user.Id,
                    AddressId = addressId
                };
                IEnumerable<ShoppingCart> shoppingCartList = _shoppingCartService.GetUserCartAsList(user.Id);

                if (shoppingCartList.Count() > 0)
                {
                    _saleService.UpdateStockEcommerce(shoppingCartList);
                    var saleSuccess = _saleService.SaveEcommerceSale(sale, shoppingCartList);
                    await _mailService.GenerateAndSendSaleEcommerceEmail(user.Id, saleSuccess);
                    return true;
                }
                return false;
            }
            return false;
        }

        [HttpGet]
        [Authorize(Policy = "CanViewSales")]
        public async Task<IActionResult> DownloadExcelReportAsync()
        {
            var content = await _documentService.GenerateExcelReport();
            var format = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var excelName = "ReporteDeVentas_"+DateTime.Now.ToShortDateString()+".xlsx";

            return File(content, format, excelName);
        }

        [HttpGet]
        [Authorize(Policy = "CanViewSales")]
        public async Task<IActionResult> DownloadPDFReportAsync()
        {
            var file = await _documentService.GeneratePdfSaleReportAsync();
            return File(file, "application/pdf", "SaleReport.pdf");
        }
    }
}
