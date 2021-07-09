using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class OrderViewModel
    {
        public int id { get; set; }
        public int cus_id { get; set; }
        public string cus_name { get; set; }
        public string name_receiver { get; set; }
        public decimal total { get; set; }
        public decimal discount { get; set; }
        public decimal transport_fee { get; set; }
        public int status { get; set; }
        public bool isPay { get; set; }
        public int ship_status { get; set; }
        public DateTime create_at { get; set; }
    }
}
