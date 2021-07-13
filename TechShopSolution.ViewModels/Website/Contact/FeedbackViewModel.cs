using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Website.Contact
{
    public class FeedbackViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string name { get; set; }
        public bool isRead { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime create_at { get; set; }
    }
}
