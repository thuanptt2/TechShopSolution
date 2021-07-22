using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class GetFavoriteProductsPagingRequest : PagingRequestBase
    {
        public int cus_id { get; set; }
    }
}
