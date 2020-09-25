using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Infrastructure.ViewModels.Dashboards.Users;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IdentityResult> ConfirmEmail(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
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
            user.EmailConfirmed = true;
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

        public void UpdatingClient(ClientEditViewModel model, User user, IFormFile image, string path)
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
                    DeleteServerFileClient(path, user.ImageRoute);
                    var imagen = UploadedServerFileClient(path, image);
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
                DeleteServerFileClient(path, user.ImageRoute);
                var imagen = UploadedServerFileClient(path, image);
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
        public ClientEditViewModel ClientToViewModel(User obj)
        {
            if (obj != null)
            {
                return new ClientEditViewModel
                {
                    Id = obj.Id,
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    Email = obj.Email,
                    Phone = obj.Phone,
                    ImageRoute = obj.ImageRoute,
                    DateOfBirth = obj.DateOfBirth
                    
                };
            }
            return null;
        }

        public async Task<IdentityResult> CreateClientAsync(User user, string password)
        {
            user.Id = Guid.NewGuid().ToString();
            user.RegisterDate = DateTime.Now;
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddingRoleToClientAsync(string User)
        {
            var user = await _userManager.FindByNameAsync(User);
            var role = await _roleManager.GetRoleByName("Cliente");
            await _userManager.AddToRoleAsync(user, role.Name);
        }

        public bool isColaborator(string role)
        {
            if (role != "Cliente")
                return true;
            return false;
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

        public async Task<string> ConfirmationEmailToken(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ChangeEmployeePassword(User user, ResetEmployeePassword newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, newPassword.Token, newPassword.Password);
        }

        public async Task<IdentityResult> ChangeClientPassword(User user, ResetPasswordModel newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, newPassword.Token, newPassword.Password);
        }

        private string UploadedServerFile(string path, IFormFile image)
        {
            string uniqueFileName = null;
            if (image != null)
            {
                string uploadsFolder = Path.Combine(path, "Images", "Users", "Employees");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private void DeleteServerFile(string path, string image)
        {
            if (image != null)
            {
                string serverFilePath = Path.Combine(path, "Images", "Users", "Employees", image);

                if (File.Exists(serverFilePath))
                    File.Delete(serverFilePath);
            }
        }

        private string UploadedServerFileClient(string path, IFormFile image)
        {
            string uniqueFileName = null;
            if (image != null)
            {
                string uploadsFolder = Path.Combine(path, "Images", "Users", "Clients");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private void DeleteServerFileClient(string path, string image)
        {
            if (image != null)
            {
                string serverFilePath = Path.Combine(path, "Images", "Users", "Clients", image);

                if (File.Exists(serverFilePath))
                    File.Delete(serverFilePath);
            }
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

       
    }
}
