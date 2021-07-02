using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.PaymentMethod
{
    public class PaymentCreateRequest
    {
        [Display(Name = "Tên phương thức")]
        public string name { get; set; }
        [Display(Name = "Mô tả phương thức")]
        public string description { get; set; }
        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }
    }
}
