using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence.EfCore.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("tb_order_items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
        builder.Property(i => i.Quantity).IsRequired();

        builder.OwnsOne(i => i.UnitPrice, m =>
        {
            m.Property(p =>
                p.Amount).HasColumnName("UnitPrice").HasColumnType("decimal(18,2)");
            m.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
        });
    }
}