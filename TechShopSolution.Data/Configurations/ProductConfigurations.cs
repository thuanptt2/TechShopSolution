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
            builder.Property(x => x.brand_id).IsRequired();
            builder.Property(x => x.code).IsRequired();
            builder.Property(x => x.isActive).IsRequired();
            builder.Property(x => x.isDelete).IsRequired();
            builder.Property(x => x.name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.promotion_price).IsRequired();
            builder.Property(x => x.featured).IsRequired();
            builder.Property(x => x.best_seller).IsRequired();
            builder.Property(x => x.unit_price).IsRequired();
            builder.Property(x => x.slug).IsRequired().HasMaxLength(255);
            builder.Property(x => x.warranty).IsRequired();
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()")
                .HasColumnType("Date");
            builder.Property(x => x.update_at)
              .HasColumnType("Date");
            builder.Property(x => x.delete_at)
               .HasColumnType("Date");
            builder.HasOne(x => x.Brand).WithMany(p => p.Products).HasForeignKey(x => x.brand_id);
        }
    }
}
