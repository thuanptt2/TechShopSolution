using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class PublicProductsViewModel
    {
        public  List<ProductViewModel> Products { get; set; }
        public int Count { get; set; }
    }
}
