using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Transport;

namespace TechShopSolution.ViewModels.Sales
{
    public class OrderDetailViewModel
    {
        public OrderModel Order { get; set; }
        public TransportViewModel Transport { get; set; }
        public List<OrderDetailModel> Details { get; set; }
    }
}
