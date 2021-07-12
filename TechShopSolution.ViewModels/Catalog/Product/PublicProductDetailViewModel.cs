using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class PublicProductDetailViewModel
    {
        public ProductViewModel Product { get; set; }
        public List<RatingViewModel> Ratings { get; set; }
    }
}
