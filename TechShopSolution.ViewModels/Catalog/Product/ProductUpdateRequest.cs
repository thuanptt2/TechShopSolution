using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class ProductUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Slug { get; set; }
        public int Brand_id { get; set; }
        public IFormFile Image { get; set; }
        public List<IFormFile> More_images { get; set; }
        public decimal Unit_price { get; set; }
        public decimal Promotion_price { get; set; }
        public int Warranty { get; set; }
        public int? Instock { get; set; }
        public string Specifications { get; set; }
        public string Short_desc { get; set; }
        public string Descriptions { get; set; }
        public bool Featured { get; set; }
        public bool Best_seller { get; set; }
        public bool IsActive { get; set; }
        public string Meta_tittle { get; set; }
        public string Meta_keywords { get; set; }
        public string Meta_descriptions { get; set; }
    }
}
