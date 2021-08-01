using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Sales
{
    public class GetCustomerOrderRequest : PagingRequestBase
    {
        public int cus_id { get; set; }
        public string filter { get; set; }
    }
}
