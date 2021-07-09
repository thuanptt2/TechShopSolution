using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Sales
{
    public class GetOrderPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
