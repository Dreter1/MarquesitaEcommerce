using FluentValidation.AspNetCore;
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

namespace MarquesitaEcommerce
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
                config.Cookie.Name = "Security.EcommerceCookie";
                config.LoginPath = "/Home/Index";
                config.AccessDeniedPath = "/Home/AccessDenied";
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
                // Client Policy
                options.AddPolicy("ClientPolicy", policy =>
                {
                    policy.RequireClaim("Permission", "Shop");
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}
