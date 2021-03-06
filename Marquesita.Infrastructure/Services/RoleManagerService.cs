﻿using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.ViewModels.Dashboards.Roles;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class RoleManagerService : IRoleManagerService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly AuthIdentityDbContext _context;

        public RoleManagerService(RoleManager<Role> roleManager, AuthIdentityDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public List<Role> GetAllRolesList()
        {
            return _roleManager.Roles.ToList();
        }

        public List<Role> GetEmployeeRolesList()
        {
            var roleList = _roleManager.Roles.ToList();
            var validRolesList = new List<Role>();
            foreach (var role in roleList)
            {
                if (role.Name != "Cliente")
                {
                    validRolesList.Add(role);
                }
            }
            return validRolesList;
        }

        public async Task<Role> GetRoleByIdAsync(string Id)
        {
            return await _roleManager.FindByIdAsync(Id);
        }

        public async Task<Role> GetRoleByName(string Name)
        {
            return await _roleManager.FindByNameAsync(Name);
        }

        public async Task<IdentityResult> CreateRoleAsync(Role role)
        {
            return await _roleManager.CreateAsync(role);
        }

        public async Task AssignPermissionsToRole(RoleViewModel model)
        {
            var role = await GetRoleByName(model.Name);

            foreach (var permission in model.Permissions)
            {
                await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }

        public async Task DeletingRoleAsync(Role role)
        {
            await _roleManager.DeleteAsync(role);
        }

        public IList<Claim> GetPermissionListOfRoleByRole(Role role)
        {
            return (_roleManager.GetClaimsAsync(role)).Result;
        }

        public List<string> PermissionListOfRole(Role role)
        {
            var PermissionList = new List<string>();
            var RolePermissions = GetPermissionListOfRoleByRole(role);
            foreach (var permission in RolePermissions)
            {
                PermissionList.Add(permission.Value);
            }
            return PermissionList;
        }

        public void UpdateRoles(RoleEditViewModel model, Role role)
        {
            role.Name = model.Name;
            role.NormalizedName = model.Name.ToUpper();
            _context.Entry(role).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public RoleEditViewModel RoleToViewModel(Role obj)
        {
            if (obj != null)
            {
                return new RoleEditViewModel
                {
                    Id = obj.Id,
                    Name = obj.Name,
                };
            }
            return null;
        }
    }
}
