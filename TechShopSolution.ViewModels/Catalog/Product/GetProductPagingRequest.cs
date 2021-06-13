using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public int? CategoryID { get; set; }
        public int? BrandID { get; set; }
    }
}
