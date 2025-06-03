using BackgroundService.Entity;
using Microsoft.EntityFrameworkCore;

namespace BackgroundService.Infra
{
    public class FinanceControlContex(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AppRole> AppRole { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<Revenues> Revenues { get; set; }
        public DbSet<PhotosUsers> PhotosUsers { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<LoginLocationData> LoginLocationData { get; set; }
        public DbSet<Notify> Notify { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }

}
