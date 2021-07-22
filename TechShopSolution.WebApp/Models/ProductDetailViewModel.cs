using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Brand;
using TechShopSolution.ViewModels.Catalog.Category;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.WebApp.Models
{
    public class ProductDetailViewModel
    {
        public ProductViewModel Product { get; set; }
        public List<RatingViewModel> Ratings { get; set; }
        public List<ProductOverViewModel> ProductsRelated { get; set; }
        public List<ProductRecentlyViewModel> ProductsRecently { get; set; }
    }
}
