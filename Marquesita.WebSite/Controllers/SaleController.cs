using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Sales;
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

        [Authorize(Policy = "CanViewSales")]
        public async Task<IActionResult> Index()
        {
            var model = await _saleService.GetOrdersAsync(this.User.Identity.Name);
            return View(model);
        }

        //[Authorize(Policy = "CanViewSales")]
        //public IActionResult IndexView()
        //{
        //    //var model = await _saleService.GetOrdersAsync(User.Identity.Name);
        //    return View(_saleService.GetSaleList());
        //}

        [Authorize(Policy = "CanAddSales")]
        public IActionResult Create()
        {

            ViewBag.Client = _usersManager.GetUsersClientsList();
            ViewBag.Employee = _usersManager.GetUsersEmployeeList();
            ViewBag.Sale = _saleService.GetSaleList();
            //ViewBag.SaleDatailTemp = 
            var model = _saleService.GetDetailTempsAsync();
            return View(model);
        }

        //[HttpPost]
        //[Authorize(Policy = "CanAddSales")]
        //public IActionResult Create(vcvcv)
        //{
        //    var model = _saleService.GetDetailTempsAsync();
        //    ViewBag.Client = _usersManager.GetUsersClientsList();
        //    ViewBag.Employee = _usersManager.GetUsersEmployeeList();
        //    ViewBag.Sale = _saleService.GetSaleList();
        //    return View(model);
        //}
        [Authorize(Policy = "CanAddSales")]
        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Products = _productService.GetComboProducts()
            };
            ViewBag.Product = _productService.GetProductList();
            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "CanAddSales")]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await _saleService.AddItemToOrderAsync(model, User.Identity.Name);
                return RedirectToAction("Create");
            }
            ViewBag.Product = _productService.GetProductList();
            return View(model);
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
            //await _saleService.UpdateStockAsync(id);
            var response = await _saleService.ConfirmOrderAsync(User.Identity.Name);
            if (response)
            {
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
