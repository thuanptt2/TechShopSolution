using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechShopSolution.WebApp.Models
{
    public class ProductRecentlyViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string image { get; set; }
        public decimal unit_price { get; set; }
        public decimal promotion_price { get; set; }
        public DateTime view_at { get; set; }
    }
}
