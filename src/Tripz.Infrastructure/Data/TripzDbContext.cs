using Microsoft.EntityFrameworkCore;
using Tripz.Domain.Entities;
using Tripz.Infrastructure.Repositories;

namespace Tripz.Infrastructure.Data
{
    public class TripzDbContext : DbContext
    {
        public TripzDbContext(DbContextOptions<TripzDbContext> options) : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nickname).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CompanyEmail).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Destination).IsRequired().HasMaxLength(200);
                entity.Property(e => e.EstimatedCost).HasPrecision(18, 2);
                entity.Property(e => e.Purpose).IsRequired().HasMaxLength(500);
                
                entity.Ignore(e => e.State);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Trips)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}