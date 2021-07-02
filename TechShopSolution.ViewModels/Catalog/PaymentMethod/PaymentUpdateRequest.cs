using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.PaymentMethod
{
    public class PaymentUpdateRequest
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }
    }
}
