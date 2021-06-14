using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Catalog.Customer
{
    public class GetCustomerPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
