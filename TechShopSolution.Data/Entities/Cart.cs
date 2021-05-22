using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Cart
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int cus_id { get; set; }
        public int quantity { get; set; }
    }
}
