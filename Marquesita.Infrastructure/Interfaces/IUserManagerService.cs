using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Infrastructure.ViewModels.Dashboards.Users;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IUserManagerService
    {
        Task<User> GetUserByNameAsync(string Name);
        Task<User> GetUserByEmailAsync(string Email);
        Task<string> GetUserIdByNameAsync(string Name);
        Task<User> GetUserByIdAsync(string Id);
        Task<IdentityResult> ConfirmEmail(User user, string token);
        Task<String> GetUserRole(User user);
        Task<List<User>> GetUsersEmployeeList();
        Task<List<User>> GetUsersClientsList();
        void UserUpdateAsync(User user);
        Task<IdentityResult> CreateUserAsync(User user, string password, IFormFile image, string path);
        Task AddingRoleToUserAsync(string UserId, string UserRol);
        void UpdatingUser(UserEditViewModel model, User user, IFormFile image, string path);
        void UpdatingClient(ClientEditViewModel model, User user, IFormFile image, string path);
        Task UpdatingRoleOfUserAsync(User user, string UserRol);
        void RemovingRestoringCredentials(User user);
        UserEditViewModel UserToViewModel(User obj);
        ClientEditViewModel ClientToViewModel(User obj);
        Task<IdentityResult> CreateClientAsync(User user, string password);
        Task AddingRoleToClientAsync(string User);
        bool EmailExists(string email);
        bool isColaborator(string role);
        Task<string> NewTokenPassword(User user);
        Task<string> ConfirmationEmailToken(User user);
        Task<IdentityResult> ChangeEmployeePassword(User user, ResetEmployeePassword newPassword);
        Task<IdentityResult> ChangeClientPassword(User user, ResetPasswordModel newPassword);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
    }
}
