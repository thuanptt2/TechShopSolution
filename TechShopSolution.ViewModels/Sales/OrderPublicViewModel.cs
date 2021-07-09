using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class OrderPublicViewModel
    {
        public int id { get; set; }
        public List<OrderDetailModel> Products { get; set; }
        public decimal total { get; set; }
        public int? order_status { get; set; }
        public int? ship_status { get; set; }
        public bool pay_status { get; set; }
    }
}
