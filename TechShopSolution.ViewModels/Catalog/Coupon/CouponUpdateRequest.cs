using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Coupon
{
    public class CouponUpdateRequest
    {
        public int id { get; set; }
        [Display(Name = "Mã giảm giá")]
        public string code { get; set; }
        [Display(Name = "Tên mã")]
        public string name { get; set; }
        [Display(Name = "Kiểu giảm")]
        public string type { get; set; }
        [Display(Name = "Giá trị")]
        public double value { get; set; }
        [Display(Name = "Giá trị đơn hàng tối thiểu")]
        public double? min_order_value { get; set; }
        [Display(Name = "Giảm tối đa")]
        public double? max_price { get; set; }
        [Display(Name = "Số lượng mã giảm giá")]
        public int? quantity { get; set; }
        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime start_at { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày kết thúc")]
        public DateTime end_at { get; set; }
    }
}
