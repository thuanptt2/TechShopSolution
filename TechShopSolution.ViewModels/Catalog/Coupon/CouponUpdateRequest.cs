using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Coupon
{
    public class CouponUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? quantity { get; set; }
        public bool isActive { get; set; }
        public DateTime start_at { get; set; }
        public DateTime end_at { get; set; }
    }
}
