using EssenceShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EssenceShop.Context
{
    public class EssenceDbContext : DbContext
    {
        public EssenceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Clients> Clients { get; set; }
        public DbSet<Clothes> Clothes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity properties and relationships here if needed
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EssenceDbContext).Assembly);
        }
    }
}
