using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class ProductOverViewModel
    {
        public int id { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
        public decimal unit_price { get; set; }
        public decimal promotion_price { get; set; }
        public string image { get; set; }
        public DateTime create_at { get; set; }
        public bool best_seller { get; set; }
        public bool featured { get; set; }
        public bool isActive { get; set; }
        public int? instock { get; set; }
        public string short_desc { get; set; }
    }
}
