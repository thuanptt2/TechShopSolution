using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class OrderDetailConfigurations : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");
            builder.HasKey(x => new { x.order_id, x.product_id });
            builder.Property(x => x.unit_price).IsRequired();
            builder.Property(x => x.quantity).IsRequired();
            builder.Property(x => x.promotion_price).IsRequired();

            builder.HasOne(x => x.Order).WithMany(t => t.OrderDetails).HasForeignKey(x => x.order_id);
            builder.HasOne(x => x.Product).WithMany(t => t.OrderDetails).HasForeignKey(x => x.product_id);
        }
    }
}
