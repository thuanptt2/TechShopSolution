using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class CategoryProduct
    {
        public int product_id { get; set; }    
        public int cate_id { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
