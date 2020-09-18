using Marquesita.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Marquesita.Infrastructure.DbContexts
{
    public class BusinessDbContext : DbContext
    {
        public BusinessDbContext(DbContextOptions<BusinessDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Comments> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(pk => pk.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(pk => pk.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).IsRequired().HasMaxLength(1000);
                entity.Property(p => p.UnitPrice).IsRequired();
                entity.Property(p => p.Stock).IsRequired();
                entity.Property(p => p.IsActive).IsRequired();
                entity.Property(p => p.CategoryId).IsRequired();

                entity.HasOne(c => c.Category)
                .WithMany(p => p.products)
                .HasForeignKey(fk => fk.CategoryId)
                .HasConstraintName("FK_Product_CategoryId")
                .IsRequired();
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(pk => pk.Id);
                entity.Property(p => p.Date).IsRequired();
                entity.Property(p => p.UserId).IsRequired();
                entity.Property(p => p.TotalAmount).IsRequired();
                entity.Property(p => p.SaleStatus).IsRequired();
                entity.Property(p => p.PaymentType).IsRequired();
                entity.Property(p => p.TypeOfSale).IsRequired();

                entity.HasOne(a => a.address)
                .WithMany(s => s.sales)
                .HasForeignKey(fk => fk.AddressId)
                .HasConstraintName("FK_Sale_AddresId");
            });

            modelBuilder.Entity<SaleDetail>(entity =>
            {
                entity.HasKey(pk => pk.Id);
                entity.Property(p => p.Quantity).IsRequired();
                entity.Property(p => p.Subtotal).IsRequired();
                entity.Property(p => p.UnitPrice).IsRequired();
                entity.Property(p => p.ProductId).IsRequired();
                entity.Property(p => p.SaleId).IsRequired();

                entity.HasOne(s => s.sale)
                .WithMany(sd => sd.saleDetails)
                .HasForeignKey(fk => fk.SaleId)
                .HasConstraintName("FK_SaleDetail_SaleId")
                .IsRequired();
                
                entity.HasOne(s => s.product)
                .WithMany(sd => sd.saleDetails)
                .HasForeignKey(fk => fk.ProductId)
                .HasConstraintName("FK_SaleDetail_ProductId")
                .IsRequired();
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.HasKey(pk => pk.Id);
                entity.Property(p => p.ProductId).IsRequired();
                entity.Property(p => p.UserId).IsRequired();

                entity.HasOne(p => p.product)
                .WithMany(wl => wl.wishLists)
                .HasForeignKey(fk => fk.ProductId)
                .HasConstraintName("FK_WishList_ProductId")
                .IsRequired();
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(pk => pk.Id);
                entity.Property(p => p.ProductId).IsRequired();
                entity.Property(p => p.UserId).IsRequired();
                entity.Property(p => p.Quantity).IsRequired();

                entity.HasOne(p => p.products)
                .WithMany(sc => sc.ShopingCartItems)
                .HasForeignKey(fk => fk.ProductId)
                .HasConstraintName("FK_ShoppingCart_ProductId")
                .IsRequired();
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(pk => pk.Id);
                entity.Property(p => p.Street).IsRequired();
                entity.Property(p => p.Conutry).IsRequired();
                entity.Property(p => p.Region).IsRequired();
                entity.Property(p => p.City).IsRequired();
                entity.Property(p => p.PostalCode).IsRequired();
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasKey(pk => pk.Id);
                entity.Property(p => p.Text).IsRequired();
                entity.Property(p => p.UserId).IsRequired();
                entity.Property(p => p.ProductId).IsRequired();

                entity.HasOne(p => p.product)
                .WithMany(c => c.comments)
                .HasForeignKey(fk => fk.ProductId)
                .HasConstraintName("FK_Comment_ProductId")
                .IsRequired();
            });
        }
    }
}
