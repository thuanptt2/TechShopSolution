using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(x => x.id);
            builder.Property(x => x.cate_name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.cate_slug).IsRequired().HasMaxLength(255).IsUnicode(false);
            builder.Property(x => x.isActive).IsRequired();
            builder.Property(x => x.parent_id).HasDefaultValue(0);
            builder.Property(x => x.isDelete).IsRequired();
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()")
                .HasColumnType("Date");
            builder.Property(x => x.update_at)
                .HasColumnType("Date");
            builder.Property(x => x.delete_at)
                .HasColumnType("Date");
        }
    }
}
