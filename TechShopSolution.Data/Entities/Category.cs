using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Category
    {
        public int id { get; set; }
        public string cate_name { get; set; }
        public string cate_slug { get; set; }
        public int? parent_id { get; set; }
        public bool status { get; set; }
        public string meta_title { get; set; }
        public string meta_keywords { get; set; }
        public string meta_descriptions { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
        public List<CategoryProduct> ProductInCategory { get; set; }

    }
}
