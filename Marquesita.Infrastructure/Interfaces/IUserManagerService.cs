using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Models.Identity;
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
        Task<string> GetUserIdByNameAsync(string Name);
        Task<User> GetUserByIdAsync(string Id);
        Task<String> GetUserRole(User user);
        List<User> GetUsersList();
        void UserUpdateAsync(User user);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task AddingRoleToUserAsync(string UserId, string UserRol);
        void UpdatingUser(UserViewModel model, User user);
        Task UpdatingRoleOfUserAsync(User user, string UserRol);
        void RemovingRestoringCredentials(User user);
        UserViewModel UserToViewModel(User obj);
        bool EmailExists(string email);
    }
}
