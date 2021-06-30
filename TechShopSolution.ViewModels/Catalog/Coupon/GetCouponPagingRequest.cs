using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Catalog.Coupon
{
    public class GetCouponPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
}
}
