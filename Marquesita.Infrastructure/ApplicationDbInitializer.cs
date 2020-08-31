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

            var role = await roleManager.FindByNameAsync("Super Admin");
            if (role == null)
            {
                await roleManager.CreateAsync(new Role { Name = "Super Admin", NormalizedName = "SUPER ADMIN" });

                var newrole = await roleManager.FindByNameAsync("Super Admin");
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Ver Usuarios"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Agregar Usuario"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Editar Usuario"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Eliminar Usuario"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Ver Roles"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Agregar Roles"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Editar Roles"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Eliminar Roles"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Ver Productos"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Agregar Productos"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Editar Productos"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Eliminar Productos"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Ver Categorias"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Agregar Categoria"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Editar Categoria"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Eliminar Categoria"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Ver Ventas"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Agregar Venta"));
                await roleManager.AddClaimAsync(newrole, new Claim("Permission", "Editar Venta"));
            }

            Thread.Sleep(300);

            if (userManager.FindByNameAsync("superadmin").Result == null)
            {
                User user = new User
                {
                    LockoutEnabled = false,
                    UserName = "superadmin",
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    Phone = "12345678",
                    DateOfBirth = new DateTime(1996, 05, 05),
                    RegisterDate = DateTime.Now
                    
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
