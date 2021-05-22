using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Brand
    {
        public int id { get; set; }
        public string brand_name { get; set; }
        public string brand_slug { get; set; }
        public bool status { get; set; }
        public string meta_title { get; set; }
        public string meta_keywords { get; set; }
        public string meta_descriptions { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }

        public List<Product> Products { get; set; }
    }
}
