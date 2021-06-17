using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class PartnerConfigurations : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.ToTable("Partner");
            builder.HasKey(x => x.id);
            builder.Property(x => x.link).IsRequired().HasMaxLength(255);
            builder.Property(x => x.image).IsRequired().HasMaxLength(255);
            builder.Property(x => x.status).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.name).IsRequired().HasMaxLength(255);
        }
    }
}
