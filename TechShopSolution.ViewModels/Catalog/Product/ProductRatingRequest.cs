using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class ProductRatingRequest
    {
        public int product_id { get; set; }
        public string product_slug { get; set; }
        public int cus_id { get; set; }
        public int score { get; set; }
        public string content { get; set; }
    }
}
