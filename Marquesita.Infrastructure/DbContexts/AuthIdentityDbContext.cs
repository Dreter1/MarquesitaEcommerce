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

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(u => u.UserName).IsRequired();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(350);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(250);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(250);
                entity.Property(u => u.Phone).IsRequired().HasMaxLength(15);
                entity.Property(u => u.ImageRoute).HasMaxLength(250);
                entity.Property(u => u.DateOfBirth).IsRequired();
                entity.Property(u => u.RegisterDate).IsRequired();
                entity.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
                entity.Property(u => u.LockoutEnabled).HasDefaultValue(false);
            });

            modelBuilder.Entity<User>()
                .Ignore(t => t.AccessFailedCount)
                .Ignore(t => t.PhoneNumber)
                .Ignore(t => t.PhoneNumberConfirmed)
                .Ignore(t => t.TwoFactorEnabled);

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("Claims");
                entity.Property(claim => claim.ClaimType).HasMaxLength(256);
                entity.Property(claim => claim.ClaimValue).HasMaxLength(256);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaim");
                entity.Property(roleClaim => roleClaim.ClaimType).HasMaxLength(256);
                entity.Property(roleClaim => roleClaim.ClaimValue).HasMaxLength(256);
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.Property(userRoles => userRoles.UserId).HasMaxLength(256);
                entity.Property(userRoles => userRoles.RoleId).HasMaxLength(256);
            });

            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
        }

    }
}
