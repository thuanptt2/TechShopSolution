using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Coupon
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public double value { get; set; }
        public int? quantity { get; set; }
        public bool isActive { get; set; }
        public DateTime start_at { get; set; }
        public DateTime end_at { get; set; }
    }
}
