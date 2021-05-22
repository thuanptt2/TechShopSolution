using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class OrderDetail
    {
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal? promotion_price { get; set; }
    }
}
