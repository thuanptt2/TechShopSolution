using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.WebApp.Models
{
    public class HomeViewModel
    {
        public PublicProductsViewModel FeaturedProducts { get; set; }
        public PublicProductsViewModel BestSellerProducts { get; set; }
        public PublicProductsViewModel ProductWithCate1 { get; set; }
        public PublicProductsViewModel ProductWithCate2 { get; set; }
        public PublicProductsViewModel ProductWithCate3 { get; set; }

    }
}
