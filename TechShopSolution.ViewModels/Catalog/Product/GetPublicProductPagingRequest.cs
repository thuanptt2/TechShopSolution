using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.Application.DTO;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
