using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechShopSolution.WebApp.Models
{
    public class CouponViewModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public double value { get; set; }
        public int? quantity { get; set; }
        public double? min_order_value { get; set; }
        public double? max_value { get; set; }
        public string coupon_message { get; set; }
    }
}
