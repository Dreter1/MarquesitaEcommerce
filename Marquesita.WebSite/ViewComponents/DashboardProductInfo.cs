using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Business;
using Microsoft.AspNetCore.Mvc;

namespace Marquesita.WebSite.ViewComponents
{
    public class DashboardProductInfo : ViewComponent
    {
        private readonly IProductService _productService;

        public DashboardProductInfo(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke(Guid ProductId)
        {
            var product =  _productService.GetProductById(ProductId);
            return View(new Product
            {
                Name = product.Name
                
            });
        }


    }
}