using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class CreteOrderRequest
    {
        public int cus_id { get; set; }
        public string name_receiver { get; set; }
        public string phone_receiver { get; set; }
        public string address_receiver { get; set; }
        public decimal total { get; set; }
        public decimal discount { get; set; }
        public decimal? transport_fee { get; set; }
        public int? coupon_id { get; set; }
        public int payment_id { get; set; }
        public string note { get; set; }
    }
}
