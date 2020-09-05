using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Infrastructure.ViewModels.Dashboards.Users;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleManagerService _roleManager;
        private readonly AuthIdentityDbContext _context;
        private readonly IConstantService _file;

        public UserManagerService(UserManager<User> userManager, IRoleManagerService roleManager, AuthIdentityDbContext context, IConstantService file)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _file = file;
        }

        public async Task<User> GetUserByNameAsync(string Name)
        {
            return await _userManager.FindByNameAsync(Name);
        }

        public async Task<User> GetUserByEmailAsync(string Email)
        {
            return await _userManager.FindByEmailAsync(Email);
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
            foreach (var user in usersList)
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

        public async Task<IdentityResult> CreateUserAsync(User user, string password, IFormFile image, string path)
        {
            var imagen = UploadedServerFile(path, image);
            user.Id = Guid.NewGuid().ToString();
            user.RegisterDate = DateTime.Now;
            user.ImageRoute = imagen;
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddingRoleToUserAsync(string User, string UserRol)
        {
            var user = await _userManager.FindByNameAsync(User);
            var role = await _roleManager.GetRoleByName(UserRol);
            await _userManager.AddToRoleAsync(user, role.Name);
        }

        public void UpdatingUser(UserEditViewModel model, User user, IFormFile image, string path)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.Phone = model.Phone;

            if (user.ImageRoute != null)
            {
                if (image != null)
                {
                    DeleteServerFile(path, user.ImageRoute);
                    var imagen = UploadedServerFile(path, image);
                    user.ImageRoute = imagen;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    user.ImageRoute = user.ImageRoute;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            else
            {
                DeleteServerFile(path, user.ImageRoute);
                var imagen = UploadedServerFile(path, image);
                user.ImageRoute = imagen;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
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
            if (obj != null)
            {
                return new UserEditViewModel
                {
                    Id = obj.Id,
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    Email = obj.Email,
                    Phone = obj.Phone,
                    ImageRoute = obj.ImageRoute,
                    DateOfBirth = obj.DateOfBirth,
                    RegisterDate = obj.RegisterDate
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

        public async Task<string> NewTokenPassword(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ChangeEmployeePassword(User user, ResetEmployeePassword newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, newPassword.Token, newPassword.Password);
        }

        private string UploadedServerFile(string path, IFormFile image)
        {
            string uniqueFileName = null;
            if (image != null)
            {
                string uploadsFolder = Path.Combine(path, "Images", "Users");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                var ruta = _file.RoutePathEmployeeImages();
                string localfilePath = ruta + uniqueFileName;

                using (var fileStream = new FileStream(localfilePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private void DeleteServerFile(string path, string image)
        {
            string ruta = _file.RoutePathEmployeeImages();
            string localFilePath = ruta + image;

            if (File.Exists(localFilePath))
                File.Delete(localFilePath);

            if (image != null)
            {
                string serverFilePath = Path.Combine(path, "Images", "Users", image);

                if (File.Exists(serverFilePath))
                    File.Delete(serverFilePath);
            }
        }
    }
}
