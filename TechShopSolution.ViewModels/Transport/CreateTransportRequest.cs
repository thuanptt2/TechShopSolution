using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Transport
{
    public class CreateTransportRequest
    {
        public int order_id { get; set; }
        [Display(Name = "Đơn vị vận chuyển")]
        [Required(ErrorMessage = "Vui lòng chọn đơn vị vận chuyển")]
        public int transporter_id { get; set; }
        [Display(Name = "Tiền thu hộ (COD)")]
        [Required(ErrorMessage = "Vui lòng nhập số tiền thu hộ")]
        public decimal? cod_price { get; set; }
        [Display(Name = "Mã vận đơn")]
        public string lading_code { get; set; }
        [Display(Name = "Địa chỉ gửi hàng")]
        [Required(ErrorMessage = "Chưa có địa chỉ gửi hàng, vui lòng cung cấp đầy đủ")]
        public string from_address { get; set; }
        [Display(Name = "Địa chỉ nhận hàng")]
        [Required(ErrorMessage = "Chưa có địa chỉ nhận hàng, vui lòng cung cấp đầy đủ")]
        public string to_address { get; set; }
    }
}
