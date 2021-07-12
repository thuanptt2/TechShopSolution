using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class RatingViewModel
    {
        public int product_id { get; set; }
        public int cus_id { get; set; }
        public string cus_name { get; set; }
        public int score { get; set; }
        public string content { get; set; }
        public DateTime date_rating { get; set; }
    }
}
