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
            builder.ToTable("Bill");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).UseIdentityColumn(100000,1);
            builder.Property(x => x.cus_id).IsRequired();
            builder.Property(x => x.payment_id).IsRequired();
            builder.Property(x => x.status).IsRequired();
            builder.Property(x => x.isPay).IsRequired();
            builder.Property(x => x.total).IsRequired();
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()");
            builder.Property(x => x.update_at);
            builder.HasOne(x => x.Customers).WithMany(t => t.Order).HasForeignKey(x => x.cus_id);
            builder.HasOne(x => x.Coupon).WithMany(t => t.Orders).HasForeignKey(x => x.coupon_id);
            builder.HasOne(x => x.PaymentMethod).WithMany(t => t.Orders).HasForeignKey(x => x.payment_id);
            builder.HasOne(x => x.Transport).WithOne(t => t.Order).HasForeignKey<Transport>(t => t.order_id);
        }
    }
}
