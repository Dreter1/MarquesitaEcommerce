﻿using Marquesita.Infrastructure.Interfaces;
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
        private readonly IConstantService _images;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAddressService _addressService;

        public ClientController(IUserManagerService usersManager, IConstantService images, IWebHostEnvironment webHostEnvironment, IAddressService addressService)
        {
            _usersManager = usersManager;
            _images = images;
            _webHostEnvironment = webHostEnvironment;
            _addressService = addressService;
        }

        [Authorize(Policy = "Client")]
        public async Task<IActionResult> MyProfileAsync()
        {
            var userProfile = await _usersManager.GetUserByNameAsync(User.Identity.Name);

            if (userProfile != null)
            {
                ViewBag.Image = _images.RoutePathRootClientsImages();
                ViewBag.UserId = userProfile.Id;
                return View(userProfile);
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [Authorize(Policy = "Client")]
        public async Task<IActionResult> Edit(string Id)
        {
            var actualId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);

            if (actualId == Id)
            {
                var userEditProfile = _usersManager.ClientToViewModel(await _usersManager.GetUserByIdAsync(Id));

                if (userEditProfile != null)
                {
                    ViewBag.Image = _images.RoutePathRootClientsImages();
                    ViewBag.UserId = actualId;
                    ViewBag.User = userEditProfile.Id;
                    return View(userEditProfile);
                }
            }
            return RedirectToAction("NotFound404", "Error");
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> Edit(ClientEditViewModel model, string Id)
        {

            var userProfile = await _usersManager.GetUserByIdAsync(Id);
            var path = _webHostEnvironment.WebRootPath;

            if (ModelState.IsValid)
            {
                if (userProfile != null)
                {
                    _usersManager.UpdatingClient(model, userProfile, model.ProfileImage, path);
                    return RedirectToAction("MyProfile", "Client");
                }
            }

            ViewBag.Image = _images.RoutePathRootClientsImages();
            ViewBag.UserId = await _usersManager.GetUserIdByNameAsync(User.Identity.Name);
            ViewBag.UserRole = await _usersManager.GetUserRole(userProfile);
            ViewBag.User = userProfile.Id;

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
        public IActionResult EditAddress(Guid Id)
        {
            var address = _addressService.AddressToViewModel(_addressService.GetAddressById(Id));

            if (address != null)
            {
                return View(address);
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
    }
}
