using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class CreteOrderRequest
    {
        public int cus_id { get; set; }
        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        public string name_receiver { get; set; }
        [Required(ErrorMessage = "Số điện thoại người nhận không được để trống")]
        public string phone_receiver { get; set; }
        [Required(ErrorMessage = "Chưa chọn địa chỉ giao hàng, vui lòng chọn đầy đủ địa chỉ")]
        public string address_receiver { get; set; }
        public decimal total { get; set; }
        public decimal discount { get; set; }
        public decimal? transport_fee { get; set; }
        public int? coupon_id { get; set; }
        public int payment_id { get; set; }
        public string note { get; set; }
    }
}
