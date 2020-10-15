using Marquesita.Infrastructure.Services;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {

            var adminRole = await roleManager.FindByNameAsync("Super Admin");
            var clientRole = await roleManager.FindByNameAsync("Cliente");
            if (adminRole == null && clientRole == null)
            {
                await roleManager.CreateAsync(new Role { Name = "Super Admin", NormalizedName = "SUPER ADMIN" });
                await roleManager.CreateAsync(new Role { Name = "Cliente", NormalizedName = "CLIENTE" });

                var newAdminRole = await roleManager.FindByNameAsync("Super Admin");
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.VIEW_USERS));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.ADD_USER));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.EDIT_USER));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.DELETE_USER));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.VIEW_ROLES));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.ADD_ROLE));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.EDIT_ROLE));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.DELETE_ROLE));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.VIEW_PRODUCTS));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.ADD_PRODUCT));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.EDIT_PRODUCT));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.DELETE_PRODUCT));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.VIEW_CATEGORYS));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.ADD_CATEGORY));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.EDIT_CATEGORY));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.DELETE_CATEGORY));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.VIEW_SALES));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.ADD_SALE));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", ConstantsService.RoleTypes.EDIT_SALE));

                var newClientRole = await roleManager.FindByNameAsync("Cliente");
                await roleManager.AddClaimAsync(newClientRole, new Claim("Permission", ConstantsService.RoleTypes.CLIENT));
            }

            Thread.Sleep(300);

            if (userManager.FindByNameAsync("superadmin").Result == null && userManager.FindByNameAsync("cliente").Result == null)
            {
                User userAdmin = new User
                {
                    LockoutEnabled = false,
                    UserName = "superadmin",
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    Phone = "123456789",
                    DateOfBirth = new DateTime(1996, 05, 05),
                    RegisterDate = DateTime.Now   
                };

                User userClient = new User
                {
                    LockoutEnabled = false,
                    UserName = "cliente",
                    FirstName = "Cliente",
                    LastName = "Cliente",
                    Email = "cliente@gmail.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "CLIENTE@GMAIL.COM",
                    Phone = "123456789",
                    DateOfBirth = new DateTime(1996, 05, 05),
                    RegisterDate = DateTime.Now
                };

                IdentityResult resultAdmin = userManager.CreateAsync(userAdmin, "password").Result;
                IdentityResult resultClient = userManager.CreateAsync(userClient, "password").Result;

                if (resultAdmin.Succeeded && resultClient.Succeeded)
                {
                    userManager.AddToRoleAsync(userAdmin, "Super Admin").Wait();
                    userManager.AddToRoleAsync(userClient, "Cliente").Wait();
                }
            }
        }
    }
}
