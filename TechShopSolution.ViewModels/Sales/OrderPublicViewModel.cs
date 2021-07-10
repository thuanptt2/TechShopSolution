using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class OrderPublicViewModel
    {
        public int id { get; set; }
        public List<OrderDetailModel> Products { get; set; }
        public decimal total { get; set; }
        public int? order_status { get; set; }
        public int? ship_status { get; set; }
        public decimal? ship_fee { get; set; }
        public decimal? discount { get; set; }
        public bool pay_status { get; set; }
        public string payment_name { get; set; }
        public string receiver_address { get; set; }
        public string receiver_name { get; set; }
        public string receiver_number { get; set; }
        public string cancel_reason { get; set; }
        public string transporter_name { get; set; }
        public string transporter_link { get; set; }
        public string lading_code { get; set; }
        public DateTime? enter_lading_code_at { get; set; }
        public DateTime? pay_at { get; set; }
        public DateTime? create_at { get; set; }
        public DateTime? confirm_at { get; set; }
        public DateTime? ship_at { get; set; }
        public DateTime? cancel_at { get; set; }
        public DateTime? cancel_ship_at { get; set; }
        public DateTime? done_ship_at { get; set; }
    }
}
