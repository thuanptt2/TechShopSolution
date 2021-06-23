using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechShopSolution.WebApp.Models
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public  string Name { get; set; }
        public  string Images { get; set; }
    }
}
