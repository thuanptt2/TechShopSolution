using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Transport
{
    public class GetTransporterPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
