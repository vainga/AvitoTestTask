using AvitoTestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace AvitoTestTask.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MerchItem> MerchItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Inventory)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
