using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marquesita.Infrastructure.DbContexts
{
    public class AuthIdentityDbContext: IdentityDbContext<User, Role, string>
    {
        public AuthIdentityDbContext(DbContextOptions<AuthIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");

                b.Property(u => u.UserName).IsRequired();
                b.Property(u => u.Email).IsRequired();
                b.Property(u => u.FirstName).IsRequired();
                b.Property(u => u.LastName).IsRequired();
                b.Property(u => u.Phone).IsRequired();
                b.Property(u => u.DateOfBirth).IsRequired();
                b.Property(u => u.RegisterDate).IsRequired();
                b.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
                b.Property(u => u.LockoutEnabled).HasDefaultValue(false);
            });

            modelBuilder.Entity<User>()
                .Ignore(t => t.AccessFailedCount)
                .Ignore(t => t.PhoneNumber)
                .Ignore(t => t.PhoneNumberConfirmed)
                .Ignore(t => t.TwoFactorEnabled);

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("Claims");
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("Roles");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaim");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles");
            });

            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
        }

    }
}
