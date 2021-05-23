using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Application.DTO;

namespace TechShopSolution.ViewModels.Catalog.Product.Manage
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public List<int> CategoryID { get; set; }

    }
}
