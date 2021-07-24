using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Favorite
    {
        public int product_id { get; set; }
        public int cus_id { get; set; }
        public DateTime date_favorite { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
