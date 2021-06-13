using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Brand
{
    public class BrandCreateRequest
    {
        public string brand_name { get; set; }
        public string brand_slug { get; set; }
        public bool isActive { get; set; }
        public string meta_title { get; set; }
        public string meta_keywords { get; set; }
        public string meta_descriptions { get; set; }
    }
}
