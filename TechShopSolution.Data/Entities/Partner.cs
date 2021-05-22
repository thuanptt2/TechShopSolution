using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Partner
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public bool status { get; set; }
        public string link { get; set; }
    }
}
