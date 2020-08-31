using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly AuthIdentityDbContext _context;

        public UserManagerService(UserManager<User> userManager, IRoleManagerService roleManager, AuthIdentityDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
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

        public async Task<List<User>> GetUsersEmployeeList()
        {
            var usersList = _userManager.Users.ToList();
            var employeeList = new List<User>();
            foreach(var user in usersList)
            {
                var role = await GetUserRole(user);
                if (role != "Cliente")
                {
                    employeeList.Add(user);
                }
            }
            return employeeList;
        }

        public async Task<List<User>> GetUsersClientsList()
        {
            var usersList = _userManager.Users.ToList();
            var clientList = new List<User>();
            foreach (var user in usersList)
            {
                var role = await GetUserRole(user);
                if (role == "Cliente")
                {
                    clientList.Add(user);
                }
            }
            return clientList;
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
            user.RegisterDate = DateTime.Now;
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddingRoleToUserAsync(string User, string UserRol)
        {
            var user = await _userManager.FindByNameAsync(User);
            var role = await _roleManager.GetRoleByName(UserRol);
            await _userManager.AddToRoleAsync(user, role.Name);
        }

        public void UpdatingUser(UserEditViewModel model, User user)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.Phone = model.Phone;
            user.ImageRoute = model.ImageRoute;

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task UpdatingRoleOfUserAsync(User user, string UserRol)
        {
            var role = await _roleManager.GetRoleByName(UserRol);
            var UserRole = await GetUserRole(user);
            if (UserRole == null)
                await _userManager.AddToRoleAsync(user, role.Name);
            else
            {
                await _userManager.RemoveFromRoleAsync(user, UserRole);
                await _userManager.AddToRoleAsync(user, role.Name);
            }
        }

        public void RemovingRestoringCredentials(User user)
        {
            if (user.IsActive)
            {
                user.IsActive = false;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                user.IsActive = true;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }

        }

        public UserEditViewModel UserToViewModel(User obj)
        {
            if(obj != null)
            {
                return new UserEditViewModel
                {
                    Id = obj.Id,
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    Email = obj.Email,
                    Phone = obj.Phone,
                    ImageRoute = obj.ImageRoute
                };

            }
            return null;
        }

        public bool EmailExists(string email)
        {
            if (_context.Users.Any(u => u.Email == email))
                return true;
            return false;
        }
    }
}
