using AutoGestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoGestAPI.Database
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderAndService> OrderAndService { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithMany(u => u.Clients)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            //
            modelBuilder.Entity<Service>()
                .HasOne(c => c.User)
                .WithMany(u => u.Services)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            //
            modelBuilder.Entity<OrderAndService>()
                .HasKey(os => new { os.OrderId, os.ServiceId });
            modelBuilder.Entity<OrderAndService>()
                .HasOne(os => os.Order)
                .WithMany(o => o.OrdersAndServices)
                .HasForeignKey(os => os.OrderId);
            modelBuilder.Entity<OrderAndService>()
                .HasOne(os => os.Service)
                .WithMany(s => s.OrderAndServices)
                .HasForeignKey(os => os.ServiceId);
            //
        }
    }
}
