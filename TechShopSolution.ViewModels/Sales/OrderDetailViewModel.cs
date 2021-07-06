using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class OrderDetailViewModel
    {
        public OrderModel Order { get; set; }
        public List<OrderDetailModel> Details { get; set; }
    }
}
