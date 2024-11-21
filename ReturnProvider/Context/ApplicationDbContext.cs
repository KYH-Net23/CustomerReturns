using Microsoft.EntityFrameworkCore;
using ReturnProvider.Models.Entities;

namespace ReturnProvider
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<ReturnEntity> Returns { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderEntity>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.HasMany(e => e.Items)
                      .WithOne()
                      .HasForeignKey(i => i.OrderId);
            });

            modelBuilder.Entity<OrderItemEntity>(entity =>
            {
                entity.HasKey(e => e.ItemId);
            });

            modelBuilder.Entity<ReturnEntity>(entity =>
            {
                entity.HasKey(e => e.ReturnId);
            });
        }
    }




    //public class ApplicationDbContext : DbContext
    //{
    //    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    //    public DbSet<ReturnEntity> Returns { get; set; }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //        // Configure Returns table
    //        modelBuilder.Entity<ReturnEntity>(entity =>
    //        {
    //            entity.HasKey(e => e.ReturnId);
    //            entity.Property(e => e.ReturnReason).HasMaxLength(255);
    //            entity.Property(e => e.ResolutionType).HasMaxLength(50);
    //            entity.Property(e => e.Status).HasMaxLength(50);
    //        });
    //    }
    //}
}
