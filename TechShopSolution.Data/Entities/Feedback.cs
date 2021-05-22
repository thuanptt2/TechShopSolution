using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Feedback
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public bool status { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime create_at { get; set; }
    }
}
