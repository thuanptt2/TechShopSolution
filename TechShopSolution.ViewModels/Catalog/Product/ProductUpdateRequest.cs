using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class ProductUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string slug { get; set; }
        public int warranty { get; set; }
        public IFormFile image { get; set; }
        public string specifications { get; set; }
        public string short_desc { get; set; }
        public string descriptions { get; set; }
        public bool featured { get; set; }
        public bool best_seller { get; set; }
        public int status { get; set; }
        public string meta_tittle { get; set; }
        public string meta_keywords { get; set; }
        public string meta_descriptions { get; set; }
    }
}
