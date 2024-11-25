using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).IsRequired().HasColumnType("bigint").ValueGeneratedOnAdd();

        builder.Property(s => s.UserId).IsRequired().HasColumnType("uuid");
        builder.Property(s => s.Branch).IsRequired().HasMaxLength(100);
        builder.Property(e => e.SaleDate)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        builder.Property(s => s.TotalSaleAmount).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(s => s.TotalSaleDiscount).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(s => s.IsCanceled).IsRequired().HasColumnType("boolean");

        builder.OwnsMany( s => s.SaleItems, saleItemBuilder =>
        {
            saleItemBuilder.ToTable("SaleItems");
            saleItemBuilder.Property(si => si.ProductId).IsRequired().HasColumnType("uuid");
            saleItemBuilder.Property(si => si.Quantity).IsRequired().HasColumnType("int");
            saleItemBuilder.Property(si => si.UnitPrice).IsRequired().HasColumnType("decimal(10,2)");
            saleItemBuilder.Property(si => si.TotalAmountWithDiscount).IsRequired().HasColumnType("decimal(10,2)");
            saleItemBuilder.Property(si => si.TotalSaleItemAmount).IsRequired().HasColumnType("decimal(10,2)");
        });
    }
}