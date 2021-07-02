using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.PaymentMethod
{
    public class PaymentCreateRequest
    {
        public string name { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }
    }
}
