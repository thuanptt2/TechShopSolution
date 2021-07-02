using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Catalog.PaymentMethod
{
    public class GetPaymentPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
