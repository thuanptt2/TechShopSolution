using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class OrderModel
    {
        public int id { get; set; }
        public int cus_id { get; set; }
        public string cus_name { get; set; }
        public string cus_email { get; set; }
        public string cus_phone { get; set; }
        public string name_receiver { get; set; }
        public string phone_receiver { get; set; }
        public string address_receiver { get; set; }
        public decimal total { get; set; }
        public decimal discount { get; set; }
        public decimal transport_fee { get; set; }
        public int? coupon_id { get; set; }
        public int payment_id { get; set; }
        public bool status { get; set; }
        public bool isPay { get; set; }
        public bool isShip { get; set; }
        public string note { get; set; }
        public DateTime create_at { get; set; }
    }
}
