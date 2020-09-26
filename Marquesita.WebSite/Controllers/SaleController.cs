using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Sales;
using Marquesita.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> AddProductSale(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _saleService.AddItemToClientOrderSaleAsync(model);
                return RedirectToAction("AddProductClientSale", "Sale", new { userId = model.UserId });
            }
            ViewBag.Product = _productService.GetProductList();
            return PartialView(model);
        }

        [HttpGet]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> Increase(Guid id, string userId)
        {
            if (id != null)
            {
                await _saleService.ModifyOrderDetailTempQuantityAsync(id, 1);
                return RedirectToAction("AddProductClientSale", "Sale", new { userId });
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpGet]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> Decrease(Guid id, string userId)
        {
            if (id != null)
            {
                await _saleService.ModifyOrderDetailTempQuantityAsync(id, -1);
                return RedirectToAction("AddProductClientSale", "Sale", new { userId });
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> DeleteItem(Guid id, string userId)
        {
            if (id != null)
            {
                await _saleService.DeleteDetailTempAsync(id);
                return RedirectToAction("AddProductClientSale", "Sale", new { userId });
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> CreateSale(string userId)
        {
            var user = await _usersManager.GetUserByIdAsync(userId);

            if (user != null)
            {
                ViewBag.User = user;
                ViewBag.Product = _productService.GetProductList();
                ViewBag.SaleDatailTemp = _saleService.GetClientSaleTempList(user.Id);
                ViewBag.PaymentList = _saleService.GetPaymentList();
                return View();
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> CreateSaleAsync(Sale sale)
        {
            var employee = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            var tempList = _saleService.GetClientSaleTempList(sale.UserId);
            // meotodo(sale: Sale, employee: User, templist: List<SaleDetailTemp>)
            return View();
        }

        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> ConfirmOrder(Guid id)
        {
            var response = await _saleService.ConfirmOrderAsync(User.Identity.Name);
            if (response)
            {
                //await _saleService.UpdateStockAsync(id);
                return RedirectToAction("Index");
            }
            
            ViewBag.Client = _usersManager.GetUsersClientsList();
            ViewBag.Employee = _usersManager.GetUsersEmployeeList();
            ViewBag.Sale = _saleService.GetSaleList();
            
            return RedirectToAction("Create");
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
