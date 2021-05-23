using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Application.DTO;

namespace TechShopSolution.ViewModels.Catalog.Product.Public
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
