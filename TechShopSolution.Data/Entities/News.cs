using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class News
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int cate_id { get; set; }
        public string img { get; set; }
        public string slug { get; set; }
        public bool status { get; set; }
        public string meta_tittle { get; set; }
        public bool? featured { get; set; }
        public string meta_keywords { get; set; }
        public string meta_descriptions { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
        public CategoryNews CategoryNews { get; set; }
    }
}
