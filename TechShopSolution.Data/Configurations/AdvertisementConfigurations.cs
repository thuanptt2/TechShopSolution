using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    public class AdvertisementConfigurations : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.ToTable("Advertisement");
            builder.HasKey(x => x.id);
            builder.Property(x => x.image).IsRequired();
            builder.Property(x => x.link).IsRequired();
        }
    }
}
