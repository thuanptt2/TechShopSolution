using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Website
{
    public class SlideViewModel
    {
        public int id { get; set; }
        public string image { get; set; }
        public int display_order { get; set; }
        public string link { get; set; }
        public bool status { get; set; }
        public DateTime create_at { get; set; }
        public DateTime? update_at { get; set; }
    }
}
