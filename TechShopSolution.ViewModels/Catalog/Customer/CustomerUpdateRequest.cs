using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Customer
{
    public class CustomerUpdateRequest
    {
        public int Id { get; set; }
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string name { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"(^[0-9\-\+]{9,15}$)", ErrorMessage = "Vui lòng nhập đúng định dạng")]
        [MinLength(10, ErrorMessage  = "Số điện thoại phải ít nhất 10 kí tự")]
        [MaxLength(12, ErrorMessage = "Số điện thoại không được vượt quá 11 kí tự")]
        public string phone { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải ít nhất 6 kí tự")]
        public string password { get; set; }
        [Display(Name = "Trạng thái")]
        public bool status { get; set; }
        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email")]
        [Display(Name = "Địa chỉ Email")]
        public string email { get; set; }
        public string address { get; set; }
        [Display(Name = "Giới tính")]
        public bool sex { get; set; }
    }
}
