using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;
using TechShopSolution.Data.Configurations;

namespace TechShopSolution.Data.EF
{
    public class TechShopDBContext : DbContext
    {
        public TechShopDBContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AboutConfigurations());
            modelBuilder.ApplyConfiguration(new AdminConfigurations());
            modelBuilder.ApplyConfiguration(new AdvertisementConfigurations());
            modelBuilder.ApplyConfiguration(new BrandConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryNewsConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryProductConfigurations());
            modelBuilder.ApplyConfiguration(new ContactConfigurations());
            modelBuilder.ApplyConfiguration(new CouponConfigurations());
            modelBuilder.ApplyConfiguration(new CustomerConfigurations());
            modelBuilder.ApplyConfiguration(new FeedbackConfigurations());
            modelBuilder.ApplyConfiguration(new NewsConfigurations());
            modelBuilder.ApplyConfiguration(new OrderConfigurations());
            modelBuilder.ApplyConfiguration(new OrderDetailConfigurations());
            modelBuilder.ApplyConfiguration(new PartnerConfigurations());
            modelBuilder.ApplyConfiguration(new PaymentMethodConfigurations());
            modelBuilder.ApplyConfiguration(new ProductConfigurations());
            modelBuilder.ApplyConfiguration(new RatingConfigurations());
            modelBuilder.ApplyConfiguration(new SlideConfigurations());
            modelBuilder.ApplyConfiguration(new TransportConfigurations());
            modelBuilder.ApplyConfiguration(new TransporterConfigurations());
            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrDetails { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CategoryNews> CategoryNews { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Transporter> Transporters { get; set; }

    }
}
