using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public string CategorySlug { get; set; }
        public string BrandSlug { get; set; }
        public int? idSortType { get; set; }
        public decimal? Lowestprice { get; set; }
        public decimal? Highestprice { get; set; }
    }
}
