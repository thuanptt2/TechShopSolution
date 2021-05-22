using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TechShopSolution.Data.EF
{
    public class TechShopDBContextFactory : IDesignTimeDbContextFactory<TechShopDBContext>
    {
        public TechShopDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json")
                .Build();

            var connectionString = configuration.GetConnectionString("TechShopSolutionDB");

            var optionsBuilder = new DbContextOptionsBuilder<TechShopDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TechShopDBContext(optionsBuilder.Options);
        }
    }
}
