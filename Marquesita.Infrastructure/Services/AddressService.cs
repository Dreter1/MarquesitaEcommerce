using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marquesita.Infrastructure.Services
{
    public class AddressService : IAddressService
    {
        private readonly IRepository<Address> _addresstRepository;
        private readonly BusinessDbContext _context;

        public AddressService(IRepository<Address> addresstRepository, BusinessDbContext context)
        {
            _addresstRepository = addresstRepository;
            _context = context;
        }

        public IEnumerable<Address> GetUserAddresses(string userId)
        {
            return _context.Addresses.Where(address => address.UserId == userId).ToList();
        }

        public Address GetAddressById(Guid Id)
        {
            return _addresstRepository.Get(Id);
        }

        public void CreateUserAddress(AddressViewModel newAddress)
        {
            newAddress.Country = "Perú";
            _addresstRepository.Add(newAddress);
            _addresstRepository.SaveChanges();
        }

        public void DeleteUserAddress(Address address)
        {
            _addresstRepository.Remove(address);
        }

        public void UpdateUserAddress(AddressEditViewModel model, Address address)
        {
            address.Street = model.Street;
            address.Region = model.Region;
            address.City = model.City;
            address.PostalCode = model.PostalCode;
            address.FullNames = model.FullNames;
            address.Phone = model.Phone;
            _addresstRepository.Update(address);
            _addresstRepository.SaveChanges();
        }

        public AddressEditViewModel AddressToViewModel(Address obj)
        {
            if (obj != null)
            {
                return new AddressEditViewModel
                {
                    Id = obj.Id,
                    Street = obj.Street,
                    Region = obj.Region,
                    City = obj.City,
                    PostalCode = obj.PostalCode,
                    FullNames = obj.FullNames,
                    Phone = obj.Phone
                };
            }
            return null;
        }

        public Address AddressEditViewModelToAddress(AddressEditViewModel obj)
        {
            if (obj != null)
            {
                return new Address
                {
                    Id = obj.Id,
                    Street = obj.Street,
                    Region = obj.Region,
                    City = obj.City,
                    PostalCode = obj.PostalCode,
                    FullNames = obj.FullNames,
                    Phone = obj.Phone
                };
            }
            return null;
        }
    }
}
