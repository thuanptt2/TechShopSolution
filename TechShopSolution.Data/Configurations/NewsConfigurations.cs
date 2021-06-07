using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class NewsConfigurations : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.ToTable("News");
            builder.HasKey(x => x.id);
            builder.Property(x => x.content).IsRequired();
            builder.Property(x => x.cate_id).IsRequired();
            builder.Property(x => x.img).IsRequired();
            builder.Property(x => x.slug).IsRequired().HasMaxLength(255).IsUnicode(false);
            builder.Property(x => x.isActive).IsRequired();
            builder.Property(x => x.title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()")
                .HasColumnType("Date");
            builder.Property(x => x.update_at)
              .HasColumnType("Date");

            builder.HasOne(x => x.CategoryNews).WithMany(t => t.ListNews).HasForeignKey(x => x.cate_id);
        }
    }
}
