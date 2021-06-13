using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Catalog.Brand
{
    public class GetBrandPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
