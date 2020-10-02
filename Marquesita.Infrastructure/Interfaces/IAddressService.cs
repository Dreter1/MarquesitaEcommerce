using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using Marquesita.Models.Business;
using System;
using System.Collections.Generic;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IAddressService
    {
        IEnumerable<Address> GetUserAddresses(string userId);
        Address GetAddressById(Guid Id);
        void CreateUserAddress(AddressViewModel newAddress);
        void DeleteUserAddress(Address address);
        void UpdateUserAddress(AddressEditViewModel model, Address address);
        AddressEditViewModel AddressToViewModel(Address obj);
        Address AddressEditViewModelToAddress(AddressEditViewModel obj);
    }
}
