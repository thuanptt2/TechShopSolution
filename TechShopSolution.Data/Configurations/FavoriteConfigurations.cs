
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class FavoriteConfigurations : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorite");
            builder.HasKey(x => new { x.product_id, x.cus_id });
            builder.Property(x => x.date_favorite)
                .HasDefaultValueSql("GetDate()");
            builder.HasOne(x => x.Product).WithMany(t => t.Favoriters).HasForeignKey(x => x.product_id);
            builder.HasOne(x => x.Customer).WithMany(t => t.FavoriteProducts).HasForeignKey(x => x.cus_id);
        }
    }
}
