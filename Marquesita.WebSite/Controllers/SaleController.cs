using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Sales;
using Marquesita.Infrastructure.ViewModels.Dashboards.Sales;
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

        [Authorize(Policy = "CanViewSales")]
        public async Task<IActionResult> Index()
        {
            var model = await _saleService.GetOrdersAsync(this.User.Identity.Name);

            return View(model);
        }

        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Client = await _usersManager.GetUsersClientsList();
            ViewBag.Employee = await _usersManager.GetUsersEmployeeList();
            ViewBag.Sale = _saleService.GetSaleList();
            ViewBag.SaleDatailTemp = _saleService.GetSaleList();
           
            return View();
        }

        [Authorize(Policy = "CanAddSales")]
        public IActionResult AddProduct()
        {
            ViewBag.Product = _productService.GetProductList();
            return PartialView();
        }
        [HttpPost]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _saleService.AddItemToOrderAsync(model, User.Identity.Name);
                return RedirectToAction("Create");
            }
            ViewBag.Product = _productService.GetProductList();
            return PartialView(model);
        }
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound404", "Error");
            }

            await _saleService.DeleteDetailTempAsync(id);
            return RedirectToAction("Create");
        }
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> Increase(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound404", "Error");
            }

            await _saleService.ModifyOrderDetailTempQuantityAsync(id, 1);
            return RedirectToAction("Create");
        }
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> Decrease(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound404", "Error");
            }

            await _saleService.ModifyOrderDetailTempQuantityAsync(id, -1);
            return RedirectToAction("Create");
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
            
            ViewBag.Client = await _usersManager.GetUsersClientsList();
            ViewBag.Employee =await  _usersManager.GetUsersEmployeeList();
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
