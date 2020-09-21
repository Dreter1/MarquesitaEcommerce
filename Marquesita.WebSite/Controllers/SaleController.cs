using Marquesita.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [Authorize(Policy = "CanViewSales")]
        public IActionResult Index()
        {
            //var model = await _saleService.GetOrdersAsync(User.Identity.Name);
            return View(_saleService.GetSaleList());
        }
        
        [Authorize(Policy = "CanViewSales")]
        public IActionResult Create()
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
