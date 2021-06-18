using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Category;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.WebApp.Models
{
    public class ProductDetailViewModel
    {
        public CategoryViewModel Category { get; set; }
        public ProductViewModel Product { get; set; }
        public List<ProductViewModel> ProductsRelated { get; set; }
        public List<ImageListResult> ImageList { get; set; }
    }
}
