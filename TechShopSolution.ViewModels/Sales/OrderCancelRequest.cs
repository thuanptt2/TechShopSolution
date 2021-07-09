
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class OrderCancelRequest
    {
        public int Id { get; set; }
        [Display(Name = "Lý do hủy đơn hàng")]
        [Required(ErrorMessage = "Vui lòng chọn lý do hủy đơn hàng")]
        public string reason { get; set; }
    }
}
