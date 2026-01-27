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

        }
    }
}
