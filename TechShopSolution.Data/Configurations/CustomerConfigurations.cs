using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class CustomerConfigurations : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(x => x.id);
            builder.Property(x => x.name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.password).IsRequired().HasMaxLength(30).IsUnicode(false);
            builder.Property(x => x.isActive).IsRequired();
            builder.Property(x => x.isDelete).IsRequired();
            builder.Property(x => x.sex).IsRequired();
            builder.Property(x => x.phone).IsRequired().HasMaxLength(20).IsUnicode(false);
            builder.Property(x => x.address).IsRequired().HasMaxLength(255);
            builder.Property(x => x.birthday).IsRequired();
            builder.Property(x => x.email).IsRequired().HasMaxLength(50).IsUnicode(false);
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()");
            builder.Property(x => x.update_at);
            builder.Property(x => x.delete_at);
        }
    }
}
