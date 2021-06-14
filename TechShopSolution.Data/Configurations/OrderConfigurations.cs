using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(x => x.id);
            builder.Property(x => x.cus_id).IsRequired();
            builder.Property(x => x.payment_id).IsRequired();
            builder.Property(x => x.status).IsRequired();
            builder.Property(x => x.isPay).IsRequired();
            builder.Property(x => x.isShip).IsRequired();
            builder.Property(x => x.subtotal).IsRequired();
            builder.Property(x => x.total).IsRequired();
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()")
                .HasColumnType("Date");
            builder.Property(x => x.update_at)
                .HasColumnType("Date");
            builder.HasOne(x => x.Customers).WithMany(t => t.Order).HasForeignKey(x => x.cus_id);
            builder.HasOne(x => x.PaymentMethod).WithMany(t => t.Orders).HasForeignKey(x => x.payment_id);
        }
    }
}
