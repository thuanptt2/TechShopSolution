using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Application.DTO;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public int? CategoryID { get; set; }
        public int? BrandID { get; set; }

    }
}
