using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class TransporterConfigurations : IEntityTypeConfiguration<Transporter>
    {
        public void Configure(EntityTypeBuilder<Transporter> builder)
        {
            builder.ToTable("Transporter");
            builder.HasKey(x => x.id);
            builder.Property(x => x.name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.status).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()")
                .HasColumnType("Date");
            builder.Property(x => x.update_at)
              .HasColumnType("Date");
        }
    }
}
