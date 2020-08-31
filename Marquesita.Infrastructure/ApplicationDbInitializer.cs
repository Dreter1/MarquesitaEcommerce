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
            if (adminRole == null || clientRole == null)
            {
                await roleManager.CreateAsync(new Role { Name = "Super Admin", NormalizedName = "SUPER ADMIN" });
                await roleManager.CreateAsync(new Role { Name = "Cliente", NormalizedName = "CLIENTE" });

                var newAdminRole = await roleManager.FindByNameAsync("Super Admin");
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Ver Usuarios"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Agregar Usuario"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Editar Usuario"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Eliminar Usuario"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Ver Roles"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Agregar Roles"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Editar Roles"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Eliminar Roles"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Ver Productos"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Agregar Productos"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Editar Productos"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Eliminar Productos"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Ver Categorias"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Agregar Categoria"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Editar Categoria"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Eliminar Categoria"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Ver Ventas"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Agregar Venta"));
                await roleManager.AddClaimAsync(newAdminRole, new Claim("Permission", "Editar Venta"));

                var newClientRole = await roleManager.FindByNameAsync("Cliente");
                await roleManager.AddClaimAsync(newClientRole, new Claim("Permission", "Compras"));
            }

            Thread.Sleep(300);

            if (userManager.FindByNameAsync("superadmin").Result == null || userManager.FindByNameAsync("cliente").Result == null)
            {
                User userAdmin = new User
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

                User userClient = new User
                {
                    LockoutEnabled = false,
                    UserName = "cliente",
                    FirstName = "Cliente",
                    LastName = "Cliente",
                    Email = "cliente@gmail.com",
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
