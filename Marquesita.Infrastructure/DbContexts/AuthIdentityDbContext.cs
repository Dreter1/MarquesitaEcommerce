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
            });

            modelBuilder.Entity<User>()
                .Ignore(t => t.AccessFailedCount)
                .Ignore(t => t.EmailConfirmed)
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
