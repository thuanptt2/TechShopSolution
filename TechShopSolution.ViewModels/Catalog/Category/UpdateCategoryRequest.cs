using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Category
{
    public class UpdateCategoryRequest
    {
        public int id { get; set; }
        public string cate_name { get; set; }
        public string cate_slug { get; set; }
        public int? parent_id { get; set; }
        public bool isActive { get; set; }
        public string meta_title { get; set; }
        public string meta_keywords { get; set; }
        public string meta_descriptions { get; set; }
    }
}
