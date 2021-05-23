using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class SlideConfigurations : IEntityTypeConfiguration<Slide>
    {
        public void Configure(EntityTypeBuilder<Slide> builder)
        {
            builder.ToTable("Slide");
            builder.HasKey(x => x.id);
            builder.Property(x => x.link).IsRequired();
            builder.Property(x => x.status).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.image).IsRequired().HasMaxLength(255);
            builder.Property(x => x.display_order).IsRequired();
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()")
                .HasColumnType("Date");
            builder.Property(x => x.update_at)
              .HasColumnType("Date");
        }
    }
}
