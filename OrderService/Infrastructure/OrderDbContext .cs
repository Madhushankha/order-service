using Microsoft.EntityFrameworkCore;
using OrderService.Model;

namespace OrderService.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options) { }

    public DbSet<OrderModel> Orders { get; set; } = null!;
    public DbSet<OrderItemModel> OrderItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderModel>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Status).HasMaxLength(50).IsRequired();
            entity.Property(o => o.OrderDate).HasColumnType("datetime").IsRequired();
            entity.Property(o => o.TotalPrice).HasColumnType("decimal(10,2)").IsRequired();

            entity.HasMany(o => o.Items)
                  .WithOne(i => i.Order)
                  .HasForeignKey(i => i.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItemModel>(entity =>
        {
            entity.ToTable("OrderItems");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ProductId).IsRequired();
            entity.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
            entity.Property(i => i.UnitPrice).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(i => i.Quantity).IsRequired();
        });
    }
}
