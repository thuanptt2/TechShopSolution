using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class TransportConfigurations : IEntityTypeConfiguration<Transport>
    {
        public void Configure(EntityTypeBuilder<Transport> builder)
        {
            builder.ToTable("Transport");
            builder.HasKey(x => x.id);
            builder.Property(x => x.cod_price).IsRequired();
            builder.Property(x => x.lading_code).IsRequired().HasMaxLength(255).IsUnicode(false);
            builder.Property(x => x.transporter_id).IsRequired();
            builder.Property(x => x.order_id).IsRequired();
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()");
            builder.Property(x => x.update_at);

            builder.HasOne(x => x.Transporter).WithMany(t => t.Transports).HasForeignKey(x => x.transporter_id);
            builder.HasOne(x => x.Order).WithOne(t => t.Transport).HasForeignKey<Order>(x => x.id);
        }
    }
}
