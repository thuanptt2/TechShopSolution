using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.Configurations
{
    public class CategoryProductConfigurations : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            builder.ToTable("CategoryProduct");
            builder.HasKey(t => new { t.cate_id, t.product_id });

            builder.HasOne(t => t.Category).WithMany(c => c.ProductInCategory).HasForeignKey(c => c.cate_id);
            builder.HasOne(t => t.Product).WithMany(c => c.ProductInCategory).HasForeignKey(c => c.product_id);

        }
    }
}
