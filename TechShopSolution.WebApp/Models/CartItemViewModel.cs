using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechShopSolution.WebApp.Models
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int? Instock { get; set; }
        public string Code { get; set; }
        public string Slug { get; set; }
        public bool isExist { get; set; }
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public int Quantity { get; set; }
        public  string Name { get; set; }
        public  string Images { get; set; }
    }
}
