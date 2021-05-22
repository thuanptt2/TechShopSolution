using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Data.Entities;

namespace TechShopSolution.Data.EF
{
    public class TechShopDBContext : DbContext
    {
        public TechShopDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrDetails { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<advertisement> Advertisements { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CategoryNews> CategoryNews { get; set; }
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
