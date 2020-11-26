using Microsoft.EntityFrameworkCore;
using RxData.Models;

namespace RxData.Data
{
    public class RxContext : DbContext
    {
        public RxContext(DbContextOptions<RxContext> options)
            : base (options)
        {
        }

        public DbSet<RxPrice> RxPrices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RxPriceUser> RxPriceUsers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RxPrice>()
                .HasOne<Vendor>(rp => rp.Vendor)
                .WithMany(v => v.RxPrices)
                .HasForeignKey(rp => rp.VendorId);

            modelBuilder.Entity<RxPriceUser>()
                .HasKey(ru => new { ru.RxPriceId, ru.UserId });

            modelBuilder.Entity<RxPriceUser>()
                .HasOne(ru => ru.RxPrice)
                .WithMany(rp => rp.RxPriceUsers)
                .HasForeignKey(ru => ru.RxPriceId);

            modelBuilder.Entity<RxPriceUser>()
                .HasOne(ru => ru.User)
                .WithMany(u => u.RxPriceUsers)
                .HasForeignKey(ru => ru.UserId);

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@test.com",
                    Role = "Admin" 
                });

            modelBuilder.Entity<RxPriceUser>()
                .HasData(new RxPriceUser { RxPriceId = 1, UserId = 1 });
        }
    }
}
