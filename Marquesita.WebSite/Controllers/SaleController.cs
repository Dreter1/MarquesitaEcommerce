using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Sales;
using Marquesita.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public SaleController(ISaleService saleService, IUserManagerService usersManager, IProductService productService)
        {
            _saleService = saleService;
            _usersManager = usersManager;
            _productService = productService;
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
            var model = new AddItemViewModel{
                Productid = Productid,
                Quantity = Quantity,
                UserId = UserId
            };
            if (_saleService.IsGreaterThan0(model.Quantity))
            {
                if (_saleService.IsProductStocked(model))
                {
                    await _saleService.AddItemToClientOrderSaleAsync(model);
                    return true;
                }
                TempData["stockError"] = "No hay la cantidad que usted solicita";
                return false;
            }
            TempData["error"] = "Porfavor ingrese una cantidad";
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
                ViewBag.PaymentList = _saleService.GetPaymentList();
                return View();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> CreateSaleAsync(Sale sale)
        {
            TempData["error"] = null;
            TempData["stockError"] = null;
            var employee = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var tempList = _saleService.GetClientSaleTempList(sale.UserId);

            if (Enumerable.Count(tempList) > 0) {
                if (_saleService.StockAvailable(tempList))
                {
                    _saleService.UpdateStock(tempList);
                    _saleService.SaveSale(employee, sale, tempList);
                    TempData["saleSuccess"] = "Se realizo la venta con exito";
                    return RedirectToAction("Index", "Sale");
                }
                TempData["stockError"] = "No contamos con el stock que solicita, vuelva a intentarlo";
                return RedirectToAction("CreateSale", "Sale", new { userId = sale.UserId });
            }
            TempData["error"] = "No puede realizar una venta sin productos";
            return RedirectToAction("CreateSale", "Sale", new { userId = sale.UserId });
        }

        [Authorize(Policy = "CanEditSales")]
        public IActionResult Edit()
        {
            return View();
        }

        [Authorize(Policy = "Client")]
        public IActionResult MyOrder()
        {
            return View();
        }
        [Authorize(Policy = "Client")]
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
