using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class CheckoutRequest
    {
        public List<CreateOrderDetailRequest> OrderDetails { get; set; }
        public CreteOrderRequest Order { get; set; }
    }
}
