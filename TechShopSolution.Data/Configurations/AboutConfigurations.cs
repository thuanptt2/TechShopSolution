using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    public class AboutConfigurations : IEntityTypeConfiguration<About>
    {
        public void Configure(EntityTypeBuilder<About> builder)
        {
            builder.ToTable("About");
            builder.HasKey(x => x.id);
            builder.Property(x => x.content).IsRequired();
            builder.Property(x => x.slug).IsRequired().HasMaxLength(255).IsUnicode(false);
            builder.Property(x => x.status).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()")
                .HasColumnType("Date");
        }
    }
}
