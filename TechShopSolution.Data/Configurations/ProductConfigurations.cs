using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(x => x.id);
            builder.Property(x => x.image).IsRequired().HasMaxLength(255);
            builder.Property(x => x.cate_id).IsRequired();
            builder.Property(x => x.brand_id).IsRequired();
            builder.Property(x => x.code).IsRequired();
            builder.Property(x => x.status).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.promotion_price).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.featured).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.best_seller).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.unit_price).IsRequired();
            builder.Property(x => x.slug).IsRequired().HasMaxLength(255);
            builder.Property(x => x.warranty).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()")
                .HasColumnType("Date");
            builder.Property(x => x.update_at)
              .HasColumnType("Date");

            builder.HasOne(x => x.Brand).WithMany(p => p.Products).HasForeignKey(x => x.brand_id);
        }
    }
}
