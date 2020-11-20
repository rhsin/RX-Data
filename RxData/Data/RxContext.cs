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
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RxPrice>()
                .HasOne<Vendor>(rp => rp.Vendor)
                .WithMany(v => v.RxPrices)
                .HasForeignKey(rp => rp.VendorId);
        }
    }
}
