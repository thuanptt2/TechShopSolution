using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Slide
    {
        public int id { get; set; }
        public string image { get; set; }
        public int display_order { get; set; }
        public string link { get; set; }
        public bool status { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
    }
}
