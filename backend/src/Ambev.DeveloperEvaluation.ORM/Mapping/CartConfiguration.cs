using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");
        builder.HasKey(c => c.Id);
        builder.Property(p => p.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(c => c.UserId)
            .IsRequired();
        builder.Property(c => c.Date)
            .IsRequired();

        builder.Property(e => e.Date)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.OwnsMany(c => c.Products, productBuilder =>
        {
            productBuilder.WithOwner().HasForeignKey("CartId"); // Assuming that we want to relate it back to Cart
            productBuilder.Property<Guid>("ProductId").IsRequired();
            productBuilder.Property<int>("Quantity").IsRequired();
            productBuilder.HasKey("ProductId", "Quantity"); // Composite key to ensure uniqueness
        });
    }
}