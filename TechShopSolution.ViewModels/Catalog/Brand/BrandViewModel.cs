using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Brand
{
    public class BrandViewModel
    {
        public int id { get; set; }
        public string brand_name { get; set; }
        public string brand_slug { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
        public string meta_title { get; set; }
        public string meta_keywords { get; set; }
        public string meta_descriptions { get; set; }
        public DateTime create_at { get; set; }
        public DateTime? update_at { get; set; }
        public DateTime? delete_at { get; set; }
    }
}
