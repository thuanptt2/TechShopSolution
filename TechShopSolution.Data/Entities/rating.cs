using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Rating
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int score { get; set; }
        public string content { get; set; }
        public DateTime date_rating { get; set; }
        public bool isActive { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public Product Product { get; set; }
    }
}
