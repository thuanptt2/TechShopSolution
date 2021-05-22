using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class CouponConfigurations : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupon");
            builder.HasKey(x => x.id);
            builder.Property(x => x.code).IsRequired().IsUnicode(false);
            builder.Property(x => x.name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.status).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.start_at)
                .HasColumnType("Date");
            builder.Property(x => x.end_at)
                .HasColumnType("Date");
        }
    }
}
