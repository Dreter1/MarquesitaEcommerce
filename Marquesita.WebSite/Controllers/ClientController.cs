using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IUserManagerService _usersManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAddressService _addressService;
        private readonly ISaleService _saleService;

        public ClientController(IUserManagerService usersManager, IWebHostEnvironment webHostEnvironment, IAddressService addressService, ISaleService saleService)
        {
            _usersManager = usersManager;
            _webHostEnvironment = webHostEnvironment;
            _addressService = addressService;
            _saleService = saleService;
        }

        [Authorize(Policy = "Client")]
        public async Task<IActionResult> MyProfileAsync()
        {
            var userProfile = await _usersManager.GetUserByNameAsync(User.Identity.Name);

            if (userProfile != null)
            {
                ViewBag.Image = ConstantsService.Images.IMG_ROUTE_CLIENT;
                ViewBag.UserId = userProfile.Id;
                return View(userProfile);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [Authorize(Policy = "Client")]
        public async Task<IActionResult> Edit()
        {
            var userId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            var userEditProfile = _usersManager.ClientToViewModel(await _usersManager.GetUserByIdAsync(userId));

            if (userEditProfile != null)
            {
                ViewBag.Image = ConstantsService.Images.IMG_ROUTE_CLIENT;
                return View(userEditProfile);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> Edit(ClientEditViewModel model)
        {

            var userProfile = await _usersManager.GetUserByIdAsync(model.Id);
            var path = _webHostEnvironment.WebRootPath;

            if (ModelState.IsValid)
            {
                if (userProfile != null)
                {
                    _usersManager.UpdatingClient(model, userProfile, model.ProfileImage, path);
                    return RedirectToAction("MyProfile", "Client");
                }
            }

            ViewBag.Image = ConstantsService.Images.IMG_ROUTE_CLIENT;
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> Addresses()
        {
            var user = await _usersManager.GetUserByNameAsync(User.Identity.Name);
            ViewBag.User = user;
            return View(_addressService.GetUserAddresses(user.Id));
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> NewAddressAsync()
        {
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> NewAddress(AddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                _addressService.CreateUserAddress(model);
                return RedirectToAction("Addresses", "Client");
            }
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> EditAddressAsync(Guid Id)
        {
            var userId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            if (_addressService.IsUserAddress(Id, userId))
            {
                var address = _addressService.AddressToViewModel(_addressService.GetAddressById(Id));

                if (address != null)
                {
                    return View(address);
                }
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public IActionResult EditAddress(AddressEditViewModel model)
        {
            var addressViewModel = _addressService.AddressEditViewModelToAddress(model);
            var address = _addressService.GetAddressById(addressViewModel.Id);

            if (ModelState.IsValid)
            {
                if (address != null)
                {
                    _addressService.UpdateUserAddress(model, address);
                    return RedirectToAction("Addresses", "Client");
                }
            }

            return View(address);
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public Boolean DeleteAddress(Guid Id)
        {
            var address = _addressService.GetAddressById(Id);

            if (address != null)
            {
                _addressService.DeleteUserAddress(address);
                return true;
            }
            return false;
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> MyOrdersAsync()
        {
            var userId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            return View(_saleService.GetClientSaleList(userId));
        }

        [HttpGet]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> MyOrderDetailAsync(Guid saleId)
        {
            var userId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            if (_saleService.IsUserSale(saleId, userId))
            {
                ViewBag.Sale = _saleService.GetSaleById(saleId);
                return View(_saleService.GetDetailSaleList(saleId));
            }
            return RedirectToAction("NotFound404", "Error");
        }
    }
}
