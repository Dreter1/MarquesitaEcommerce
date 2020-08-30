using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {

            var role = await roleManager.FindByNameAsync("Super Admin");
            if (role == null)
            {
                await roleManager.CreateAsync(new Role { Name = "Super Admin", NormalizedName = "SUPER ADMIN" });
            }

            Thread.Sleep(300);

            if (userManager.FindByNameAsync("superadmin").Result == null)
            {
                User user = new User
                {
                    LockoutEnabled = false,
                    IsActive = true,
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",     
                    UserName = "superadmin"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Super Admin").Wait();
                }
            }
        }
    }
}
