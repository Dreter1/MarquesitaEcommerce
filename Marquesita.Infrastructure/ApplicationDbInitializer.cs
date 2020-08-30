using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
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

                var newrole = await roleManager.FindByNameAsync("Super Admin");
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "ViewUsers"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "AddUsers"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "EditUsers"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "DeleteUsers"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "ViewRoles"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "AddRoles"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "EditRoles"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "DeleteRoles"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "ViewProducts"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "AddProducts"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "EditProducts"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "DeleteProducts"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "ViewCategory"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "AddCategory"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "EditCategory"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "DeleteCategory"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "ViewSales"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "AddSales"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "EditSales"));
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
