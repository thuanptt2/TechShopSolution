using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ViewModels.Website.Contact
{
    public class GetFeedbackPagingRequets : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
