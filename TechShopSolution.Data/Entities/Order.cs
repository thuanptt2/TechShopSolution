using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Order
    {
        public int id { get; set; }
        public int cus_id { get; set; }
        public string name_receiver { get; set; }
        public string phone_receiver { get; set; }
        public string address_receiver { get; set; }
        public decimal total { get; set; }
        public decimal discount { get; set; }
        public decimal transport_fee { get; set; }
        public int? coupon_id { get; set; }
        public int payment_id { get; set; }
        public int status { get; set; }
        public bool isPay { get; set; }
        public string note { get; set; }
        public DateTime create_at { get; set; }
        public DateTime? confirm_at { get; set; }
        public DateTime? pay_at { get; set; }
        public DateTime? update_at { get; set; }
        public DateTime? cancel_at { get; set; }
        public string cancel_reason { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public Customer Customers { get; set; }
        public Transport Transport { get; set; }
        public Coupon Coupon { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
