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

            var adminRole = await roleManager.FindByNameAsync(ConstantsService.UserType.ADMINISTRATOR);
            var clientRole = await roleManager.FindByNameAsync(ConstantsService.UserType.CLIENT);
            if (adminRole == null && clientRole == null)
            {
                await roleManager.CreateAsync(new Role { Name = ConstantsService.UserType.ADMINISTRATOR, NormalizedName = ConstantsService.UserType.ADMINISTRATOR_UPPERCASE });
                await roleManager.CreateAsync(new Role { Name = ConstantsService.UserType.CLIENT, NormalizedName = ConstantsService.UserType.CLIENT_UPPERCASE });

                var newAdminRole = await roleManager.FindByNameAsync(ConstantsService.UserType.ADMINISTRATOR);
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

                var newClientRole = await roleManager.FindByNameAsync(ConstantsService.UserType.CLIENT);
                await roleManager.AddClaimAsync(newClientRole, new Claim("Permission", ConstantsService.RoleTypes.CLIENT));
            }

            Thread.Sleep(300);

            if (userManager.FindByNameAsync(ConstantsService.InitialsUsers.ADMIN_USERNAME).Result == null && userManager.FindByNameAsync(ConstantsService.InitialsUsers.CLIENT_USERNAME).Result == null)
            {
                User userAdmin = new User
                {
                    LockoutEnabled = ConstantsService.InitialsUsers.LOCKOUT_ENABLED,
                    UserName = ConstantsService.InitialsUsers.ADMIN_USERNAME,
                    FirstName = ConstantsService.InitialsUsers.ADMIN_FIRSTNAME,
                    LastName = ConstantsService.InitialsUsers.ADMIN_LASTNAME,
                    Email = ConstantsService.InitialsUsers.ADMIN_EMAIL,
                    EmailConfirmed = ConstantsService.InitialsUsers.EMAIL_CONFIRMED,
                    NormalizedEmail = ConstantsService.InitialsUsers.ADMIN_EMAIL_UPPERCASE,
                    Phone = ConstantsService.InitialsUsers.PHONE,
                    DateOfBirth = new DateTime(1996, 05, 05),
                    RegisterDate = DateTime.Now   
                };

                User userClient = new User
                {
                    LockoutEnabled = ConstantsService.InitialsUsers.LOCKOUT_ENABLED,
                    UserName = ConstantsService.InitialsUsers.CLIENT_USERNAME,
                    FirstName = ConstantsService.InitialsUsers.CLIENT_FIRSTNAME,
                    LastName = ConstantsService.InitialsUsers.CLIENT_LASTNAME,
                    Email = ConstantsService.InitialsUsers.CLIENT_EMAIL,
                    EmailConfirmed = ConstantsService.InitialsUsers.EMAIL_CONFIRMED,
                    NormalizedEmail = ConstantsService.InitialsUsers.CLIENT_EMAIL_UPPERCASE,
                    Phone = ConstantsService.InitialsUsers.PHONE,
                    DateOfBirth = new DateTime(1996, 05, 05),
                    RegisterDate = DateTime.Now
                };

                IdentityResult resultAdmin = userManager.CreateAsync(userAdmin, ConstantsService.InitialsUsers.PASSWORD).Result;
                IdentityResult resultClient = userManager.CreateAsync(userClient, ConstantsService.InitialsUsers.PASSWORD).Result;

                if (resultAdmin.Succeeded && resultClient.Succeeded)
                {
                    userManager.AddToRoleAsync(userAdmin, ConstantsService.UserType.ADMINISTRATOR).Wait();
                    userManager.AddToRoleAsync(userClient, ConstantsService.UserType.CLIENT).Wait();
                }
            }
        }
    }
}
