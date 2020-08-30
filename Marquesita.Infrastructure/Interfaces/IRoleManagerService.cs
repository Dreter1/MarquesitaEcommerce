using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IRoleManagerService
    {
        List<Role> GetRolesList();
        Task<Role> GetRoleByIdAsync(string Id);
        Task<Role> GetRoleByName(string Name);
        Task<IdentityResult> CreateRoleAsync(Role role);
        //Task AssignPermissionsToRole(RoleViewModel model);
        Task DeletingRoleAsync(Role role);
        IList<Claim> GetPermissionListOfRoleByRole(Role role);
        List<string> PermissionListOfRole(Role role);
        //void UpdateRoles(RoleViewModel model, Role role);
    }
}
