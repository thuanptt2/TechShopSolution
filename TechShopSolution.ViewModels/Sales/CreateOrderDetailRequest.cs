using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class CreateOrderDetailRequest
    {
        public int product_id { get; set; }
        public int quantity { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public decimal unit_price { get; set; }
        public decimal? promotion_price { get; set; }
    }
}
