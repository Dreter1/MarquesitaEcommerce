using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using Marquesita.Infrastructure;
using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Infrastructure.Repositories;
using Marquesita.Infrastructure.Services;
using Marquesita.Infrastructure.ViewModels.Dashboards.Category;
using Marquesita.Infrastructure.ViewModels.Dashboards.Roles;
using Marquesita.Infrastructure.ViewModels.Dashboards.Sales;
using Marquesita.Infrastructure.ViewModels.Dashboards.Users;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using Marquesita.Models.Business;
using Marquesita.Models.Identity;
using Marquesita.WebSite.Validators.ClientValidator;
using Marquesita.WebSite.Validators.ProductValidator;
using Marquesita.WebSite.Validators.SaleValidator;
using MarquesitaDashboards.Validators.CategoryValidator;
using MarquesitaDashboards.Validators.RoleValidator;
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
using Marquesita.WebSite.PDFUtility;
using Microsoft.AspNetCore.Http.Features;
using Marquesita.Infrastructure.EmailConfigurations.Models;
using Marquesita.Infrastructure.EmailConfigurations.Interfaces;
using Marquesita.Infrastructure.EmailConfigurations.Services;

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
            ServicesConfiguration(services);
            EmailConfiguration(services);
            PdfConfiguration(services);
        }

        private void MvcConfiguration(IServiceCollection services)
        {
            services.AddMvc().AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("/{0}.cshtml");
            })
                .AddFluentValidation()
                .AddSessionStateTempDataProvider(); ;
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

            //services.AddDbContext<BusinessDbContext>(opt =>
            //opt.UseMySQL(Configuration.GetConnectionString("BusinessDB"),
            //b => b.MigrationsAssembly("Marquesita.Infrastructure")).EnableSensitiveDataLogging()
            //);

            //services.AddDbContext<AuthIdentityDbContext>(opt =>
            //opt.UseMySQL(Configuration.GetConnectionString("IdentityDB"),
            //b => b.MigrationsAssembly("Marquesita.Infrastructure")).EnableSensitiveDataLogging()
            //);
        }

        private void IdentityConfiguration(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(config =>
            {
                config.User.RequireUniqueEmail = false;
                config.SignIn.RequireConfirmedEmail = true;
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
                config.LoginPath = "/Home/";
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
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.VIEW_USERS);
                });

                options.AddPolicy("CanAddUsers", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.ADD_USER);
                });

                options.AddPolicy("CanEditUsers", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.EDIT_USER);
                });

                options.AddPolicy("CanDeleteUsers", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.DELETE_USER);
                });

                // Roles Policy
                options.AddPolicy("CanViewRoles", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.VIEW_ROLES);
                });

                options.AddPolicy("CanAddRoles", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.ADD_ROLE);
                });

                options.AddPolicy("CanEditRoles", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.EDIT_ROLE);
                });

                options.AddPolicy("CanDeleteRoles", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.DELETE_ROLE);
                });

                // Products Policy
                options.AddPolicy("CanViewProducts", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.VIEW_PRODUCTS);
                });

                options.AddPolicy("CanAddProducts", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.ADD_PRODUCT);
                });

                options.AddPolicy("CanEditProducts", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.EDIT_PRODUCT);
                });

                options.AddPolicy("CanDeleteProducts", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.DELETE_PRODUCT);
                });

                // Categories Policy
                options.AddPolicy("CanViewCategory", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.VIEW_CATEGORYS);
                });

                options.AddPolicy("CanAddCategory", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.ADD_CATEGORY);
                });

                options.AddPolicy("CanEditCategory", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.EDIT_CATEGORY);
                });

                options.AddPolicy("CanDeleteCategory", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.DELETE_CATEGORY);
                });

                // Sales Policy
                options.AddPolicy("CanViewSales", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.VIEW_SALES);
                });

                options.AddPolicy("CanAddSales", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.ADD_SALE);
                });

                options.AddPolicy("CanEditSales", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.EDIT_SALE);
                });

                // Client Policy
                options.AddPolicy("Client", policy =>
                {
                    policy.RequireClaim("Permission", ConstantsService.RoleTypes.CLIENT);
                });
            });
        }

        private void ValidatorsConfiguration(IServiceCollection services)
        {
            services.AddTransient<IValidator<UserViewModel>, UserViewModelValidator>();
            services.AddTransient<IValidator<UserEditViewModel>, UserEditViewModelValidator>();

            services.AddTransient<IValidator<RoleViewModel>, RoleViewModelValidator>();
            services.AddTransient<IValidator<RoleEditViewModel>, RoleEditViewModelValidator>();

            services.AddTransient<IValidator<ClientViewModel>, ClientViewModelValidator>();
            services.AddTransient<IValidator<ClientEditViewModel>, ClientEditViewModelValidator>();

            services.AddTransient<IValidator<CategoryViewModel>, CategoryViewModelValidator>();
            services.AddTransient<IValidator<ProductViewModel>, ProductViewModelValidator>();
            services.AddTransient<IValidator<ProductEditViewModel>, ProductEditViewModelValidator>();
            services.AddTransient<IValidator<AddItemViewModel>, AddItemViewModelValidator>();
            services.AddTransient<IValidator<AddressViewModel>, AddressViewModelValidator>();
            services.AddTransient<IValidator<AddressEditViewModel>, AddressEditViewModelValidator>();
        }

        private void RepositoriesConfiguration(IServiceCollection services)
        {
            services.AddTransient<IRepository<Categories>, CategoryRepository>();
            services.AddTransient<IRepository<Product>, ProductRepository>();
            services.AddTransient<IRepository<Sale>, SaleRepository>();
            services.AddTransient<IRepository<SaleDetailTemp>, SaleDetailTempRepository>();
            services.AddTransient<IRepository<SaleDetail>, SaleDetailRepository>();
            services.AddTransient<IRepository<ShoppingCart>, ShoppingCartRepository>();
            services.AddTransient<IRepository<WishList>, WishListRepository>();
            services.AddTransient<IRepository<Address>, AddressRepositroy>();
        }

        private void ServicesConfiguration(IServiceCollection services)
        {
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ISaleService, SaleService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IWishListService, WishListService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IDocumentsService, DocumentsService>();
            services.AddTransient<IMailService, MailService>();

            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IRoleManagerService, RoleManagerService>();
            services.AddScoped<IAuthManagerService, AuthManagerService>();
            services.AddScoped<IConstantsService, ConstantsService>();
        }

        private void EmailConfiguration(IServiceCollection services)
        {
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IEmailsTextService, EmailsTextService>();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        private void PdfConfiguration(IServiceCollection services)
        {
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "PDFUtility", "Libraries", "libwkhtmltox.dll"));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
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
