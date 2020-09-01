using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IUserManagerService
    {
        Task<User> GetUserByNameAsync(string Name);
        Task<User> GetUserByEmailAsync(string Email);
        Task<string> GetUserIdByNameAsync(string Name);
        Task<User> GetUserByIdAsync(string Id);
        Task<String> GetUserRole(User user);
        Task<List<User>> GetUsersEmployeeList();
        Task<List<User>> GetUsersClientsList();
        void UserUpdateAsync(User user);
        Task<IdentityResult> CreateUserAsync(User user, string password, IFormFile image, string path);
        Task AddingRoleToUserAsync(string UserId, string UserRol);
        void UpdatingUser(UserEditViewModel model, User user, IFormFile image, string path);
        Task UpdatingRoleOfUserAsync(User user, string UserRol);
        void RemovingRestoringCredentials(User user);
        UserEditViewModel UserToViewModel(User obj);
        bool EmailExists(string email);
        Task<string> NewTokenPassword(User user);
        Task<IdentityResult> ChangeEmployeePassword(User user, ResetEmployeePassword newPassword);
    }
}
