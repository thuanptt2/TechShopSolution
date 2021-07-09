using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Catalog.Category;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class PublicCayegoyProductsViewModel : PublicProductsViewModel
    {
        public CategoryViewModel Category { get; set; }
    }
}
