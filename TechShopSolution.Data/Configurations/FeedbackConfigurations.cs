using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    class FeedbackConfigurations : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("FeedBack");
            builder.HasKey(x => x.id);
            builder.Property(x => x.content).IsRequired().IsUnicode();
            builder.Property(x => x.email).IsRequired().HasMaxLength(255).IsUnicode(false);
            builder.Property(x => x.isRead).IsRequired();
            builder.Property(x => x.name).IsRequired().HasMaxLength(255).IsUnicode(true);
            builder.Property(x => x.phone).IsRequired().HasMaxLength(20).IsUnicode(false);
            builder.Property(x => x.title).IsRequired().HasMaxLength(255).IsUnicode(true);
            builder.Property(x => x.create_at)
                .HasDefaultValueSql("GetDate()")
                .HasColumnType("Date");
        }
    }
}
