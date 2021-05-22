using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Customer
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public DateTime birthday { get; set; }
        public bool sex { get; set; }
        public string address { get; set; }
        public bool status { get; set; }
        public DateTime create_at { get; set; }
        public System.DateTime update_at { get; set; }
        public List<Order> Order { get; set; }
    }
}
