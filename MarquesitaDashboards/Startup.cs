using FluentValidation.AspNetCore;
using Marquesita.Infrastructure;
using Marquesita.Infrastructure.DbContexts;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MotleyFlash;
using MotleyFlash.AspNetCore.MessageProviders;
using System;
using Microsoft.AspNetCore.Identity;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Services;

namespace MarquesitaDashboards
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
            }).AddEntityFrameworkStores<AuthIdentityDbContext>();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Security.DashboardsCookie";
                config.LoginPath = "/Auth/SignIn";
                config.AccessDeniedPath = "/Auth/AccessDenied";
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
                    policy.RequireClaim("Permission", "ViewUsers");
                });

                options.AddPolicy("CanAddUsers", policy =>
                {
                    policy.RequireClaim("Permission", "AddUsers");
                });

                options.AddPolicy("CanEditUsers", policy =>
                {
                    policy.RequireClaim("Permission", "EditUsers");
                });

                options.AddPolicy("CanDeleteUsers", policy =>
                {
                    policy.RequireClaim("Permission", "DeleteUsers");
                });

                // Roles Policy
                options.AddPolicy("CanViewRoles", policy =>
                {
                    policy.RequireClaim("Permission", "ViewRoles");
                });

                options.AddPolicy("CanAddRoles", policy =>
                {
                    policy.RequireClaim("Permission", "AddRoles");
                });

                options.AddPolicy("CanEditRoles", policy =>
                {
                    policy.RequireClaim("Permission", "EditRoles");
                });

                options.AddPolicy("CanDeleteRoles", policy =>
                {
                    policy.RequireClaim("Permission", "DeleteRoles");
                });

                // Products Policy
                options.AddPolicy("CanViewProducts", policy =>
                {
                    policy.RequireClaim("Permission", "ViewProducts");
                });

                options.AddPolicy("CanAddProducts", policy =>
                {
                    policy.RequireClaim("Permission", "AddProducts");
                });

                options.AddPolicy("CanEditProducts", policy =>
                {
                    policy.RequireClaim("Permission", "EditProducts");
                });

                options.AddPolicy("CanDeleteProducts", policy =>
                {
                    policy.RequireClaim("Permission", "DeleteProducts");
                });

                // Category Policy
                options.AddPolicy("CanViewCategory", policy =>
                {
                    policy.RequireClaim("Permission", "ViewCategory");
                });

                options.AddPolicy("CanAddCategory", policy =>
                {
                    policy.RequireClaim("Permission", "AddCategory");
                });

                options.AddPolicy("CanEditCategory", policy =>
                {
                    policy.RequireClaim("Permission", "EditCategory");
                });

                options.AddPolicy("CanDeleteCategory", policy =>
                {
                    policy.RequireClaim("Permission", "DeleteCategory");
                });

                // Sales Policy
                options.AddPolicy("CanViewSales", policy =>
                {
                    policy.RequireClaim("Permission", "ViewSales");
                });

                options.AddPolicy("CanAddSales", policy =>
                {
                    policy.RequireClaim("Permission", "AddSales");
                });

                options.AddPolicy("CanEditSales", policy =>
                {
                    policy.RequireClaim("Permission", "EditSales");
                });
            });
        }

        private void ValidatorsConfiguration(IServiceCollection services)
        {
            //services.AddTransient<IValidator<UserViewModel>, UserViewModelValidator>();
            //services.AddTransient<IValidator<UserViewModel>, UserEditViewModelValidator>();

            //services.AddTransient<IValidator<RoleViewModel>, RoleViewModelValidator>();
            //services.AddTransient<IValidator<RoleViewModel>, RoleEditViewModelValidator>();

            //services.AddTransient<IValidator<PermissionViewModel>, PermissionViewModelValidator>();

        }

        private void RepositoriesConfiguration(IServiceCollection services)
        {
            //services.AddTransient<IRepository<Permission>, PermissionRepository>();
            services.AddTransient<IUserManagerService, UserManagerService>();
            services.AddTransient<IRoleManagerService, RoleManagerService>();
            services.AddTransient<IAuthManagerService, AuthManagerService>();
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
