using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Category;
using TechShopSolution.ViewModels.Catalog.Product;

namespace TechShopSolution.WebApp.Models
{
    public class HomeProductsCategoryViewModel
    {
        public CategoryViewModel Category { get; set; }
        public PublicProductsViewModel Products { get; set; }
    }
}
