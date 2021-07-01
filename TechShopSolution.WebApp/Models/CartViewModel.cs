using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechShopSolution.WebApp.Models
{
    public class CartViewModel
    {
        public List<CartItemViewModel> items { get; set; }
        public CouponViewModel coupon { get; set; }
    }
}
