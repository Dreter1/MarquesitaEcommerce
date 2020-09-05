using FluentValidation;
using FluentValidation.AspNetCore;
using Marquesita.Infrastructure;
using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Repositories;
using Marquesita.Infrastructure.Services;
using Marquesita.Infrastructure.ViewModels.Dashboards.Category;
using Marquesita.Infrastructure.ViewModels.Dashboards.Roles;
using Marquesita.Infrastructure.ViewModels.Dashboards.Users;
using Marquesita.Models.Business;
using Marquesita.Models.Identity;
using MarquesitaDashboards.Validators.CategoryValidators;
using MarquesitaDashboards.Validators.RoleValidatos;
using MarquesitaDashboards.Validators.UserValidator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MotleyFlash;
using MotleyFlash.AspNetCore.MessageProviders;
using System;

namespace MarquesitaDashboards
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            MvcConfiguration(services);
            services.AddSession();
            FlashMessagesConfiguration(services);
            DbConnectionsConfiguration(services);
            IdentityConfiguration(services);
            PoliciesConfiguration(services);
            ValidatorsConfiguration(services);
            RepositoriesConfiguration(services);
        }

        private void MvcConfiguration(IServiceCollection services)
        {
            services.AddMvc().AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("/{0}.cshtml");
            }).AddFluentValidation();
        }

        private void DbConnectionsConfiguration(IServiceCollection services)
        {
            services.AddDbContext<BusinessDbContext>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("BusinessDB"),
            b => b.MigrationsAssembly("Marquesita.Infrastructure")).EnableSensitiveDataLogging()
            );

            services.AddDbContext<AuthIdentityDbContext>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("IdentityDB"),
            b => b.MigrationsAssembly("Marquesita.Infrastructure")).EnableSensitiveDataLogging()
            );
        }

        private void IdentityConfiguration(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(config =>
            {
                config.User.RequireUniqueEmail = false;
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AuthIdentityDbContext>().AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2)
            );

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Security.MarquesitaCookie";
                config.LoginPath = "/Home";
                config.AccessDeniedPath = "/Error/AccessDenied";
                config.SlidingExpiration = true;
                config.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });
        }

        private void FlashMessagesConfiguration(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(x => x.GetRequiredService<IHttpContextAccessor>().HttpContext.Session);
            services.AddScoped<IMessageProvider, SessionMessageProvider>();

            services.AddScoped<IMessageTypes>(x =>
            {
                return new MessageTypes(error: "danger", information: "info");
            });

            services.AddScoped<IMessengerOptions, MessengerOptions>();

            services.AddScoped<IMessenger, StackMessenger>();
        }

        private void PoliciesConfiguration(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // User Policy
                options.AddPolicy("CanViewUsers", policy =>
                {
                    policy.RequireClaim("Permission", "Ver Usuarios");
                });

                options.AddPolicy("CanAddUsers", policy =>
                {
                    policy.RequireClaim("Permission", "Agregar Usuario");
                });

                options.AddPolicy("CanEditUsers", policy =>
                {
                    policy.RequireClaim("Permission", "Editar Usuario");
                });

                options.AddPolicy("CanDeleteUsers", policy =>
                {
                    policy.RequireClaim("Permission", "Eliminar Usuario");
                });

                // Roles Policy
                options.AddPolicy("CanViewRoles", policy =>
                {
                    policy.RequireClaim("Permission", "Ver Roles");
                });

                options.AddPolicy("CanAddRoles", policy =>
                {
                    policy.RequireClaim("Permission", "Agregar Roles");
                });

                options.AddPolicy("CanEditRoles", policy =>
                {
                    policy.RequireClaim("Permission", "Editar Roles");
                });

                options.AddPolicy("CanDeleteRoles", policy =>
                {
                    policy.RequireClaim("Permission", "Eliminar Roles");
                });

                // Products Policy
                options.AddPolicy("CanViewProducts", policy =>
                {
                    policy.RequireClaim("Permission", "Ver Productos");
                });

                options.AddPolicy("CanAddProducts", policy =>
                {
                    policy.RequireClaim("Permission", "Agregar Productos");
                });

                options.AddPolicy("CanEditProducts", policy =>
                {
                    policy.RequireClaim("Permission", "Editar Productos");
                });

                options.AddPolicy("CanDeleteProducts", policy =>
                {
                    policy.RequireClaim("Permission", "Eliminar Productos");
                });

                // Categories Policy
                options.AddPolicy("CanViewCategory", policy =>
                {
                    policy.RequireClaim("Permission", "Ver Categorias");
                });

                options.AddPolicy("CanAddCategory", policy =>
                {
                    policy.RequireClaim("Permission", "Agregar Categoria");
                });

                options.AddPolicy("CanEditCategory", policy =>
                {
                    policy.RequireClaim("Permission", "Editar Categoria");
                });

                options.AddPolicy("CanDeleteCategory", policy =>
                {
                    policy.RequireClaim("Permission", "Eliminar Categoria");
                });

                // Sales Policy
                options.AddPolicy("CanViewSales", policy =>
                {
                    policy.RequireClaim("Permission", "Ver Ventas");
                });

                options.AddPolicy("CanAddSales", policy =>
                {
                    policy.RequireClaim("Permission", "Agregar Venta");
                });

                options.AddPolicy("CanEditSales", policy =>
                {
                    policy.RequireClaim("Permission", "Editar Venta");
                });

                // Client Policy
                options.AddPolicy("ClientPolicy", policy =>
                {
                    policy.RequireClaim("Permission", "Compras");
                });
            });
        }

        private void ValidatorsConfiguration(IServiceCollection services)
        {
            services.AddTransient<IValidator<UserViewModel>, UserViewModelValidator>();
            services.AddTransient<IValidator<UserEditViewModel>, UserEditViewModelValidator>();

            services.AddTransient<IValidator<RoleViewModel>, RoleViewModelValidator>();
            services.AddTransient<IValidator<RoleEditViewModel>, RoleEditViewModelValidator>();

            services.AddTransient<IValidator<CategoryViewModel>, CategoryViewModelValidator>();
        }

        private void RepositoriesConfiguration(IServiceCollection services)
        {
            services.AddTransient<IRepository<Categories>, CategoryRepository>();

            services.AddTransient<ICategoryService, CategoryService>();

            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IRoleManagerService, RoleManagerService>();
            services.AddScoped<IAuthManagerService, AuthManagerService>();
            services.AddScoped<IConstantService, ConstantService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            ApplicationDbInitializer.SeedUsers(userManager, roleManager).Wait();
        }
    }
}
