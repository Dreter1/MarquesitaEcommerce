using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleManagerService _roleManager;

        public UserManagerService(UserManager<User> userManager, IRoleManagerService roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<User> GetUserByNameAsync(string Name)
        {
            return await _userManager.FindByNameAsync(Name);
        }

        public async Task<string> GetUserIdByNameAsync(string Name)
        {
            return (await _userManager.FindByNameAsync(Name)).Id;
        }

        public async Task<User> GetUserByIdAsync(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }

        public List<User> GetUsersList()
        {
            return _userManager.Users.ToList();
        }

        public void UserUpdateAsync(User user)
        {
            _userManager.UpdateAsync(user);
        }

        public async Task<String> GetUserRole(User user)
        {
            return (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            user.Id = Guid.NewGuid().ToString();
            user.IsActive = true;
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddingRoleToUserAsync(string User, string UserRol)
        {
            var user = await _userManager.FindByNameAsync(User);
            var role = await _roleManager.GetRoleByName(UserRol);
            await _userManager.AddToRoleAsync(user, role.Name);
        }
    }
}
