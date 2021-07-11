using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string slug { get; set; }
        public int brand_id { get; set; }
        public string image { get; set; }
        public string more_images { get; set; }
        public decimal unit_price { get; set; }
        public decimal promotion_price { get; set; }
        public int warranty { get; set; }
        public int? instock { get; set; }
        public string specifications { get; set; }
        public string short_desc { get; set; }
        public string descriptions { get; set; }
        public bool featured { get; set; }
        public bool best_seller { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
        public string meta_tittle { get; set; }
        public string meta_keywords { get; set; }
        public string meta_descriptions { get; set; }
        public DateTime create_at { get; set; }
        public DateTime? update_at { get; set; }
        public DateTime? delete_at { get; set; }
        public Brand Brand { get; set; }
        public List<CategoryProduct> ProductInCategory { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
