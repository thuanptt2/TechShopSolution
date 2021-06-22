using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Customer
{
    public class CustomerRegisterRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập họ")]
        [Display(Name = "Họ")]
        public string lastname { get; set; }
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Vui lòng nhập Tên")]
        public string firstname { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [Display(Name = "Email")]
        [Remote(action: "VerifyEmail", controller: "Account")]
        public string email { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string password { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [Compare(nameof(password), ErrorMessage = "Mật khẩu không trùng khớp")]
        public string confirmpassword { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string phone { get; set; }
        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        [Display(Name = "Giới tính")]
        public bool sex { get; set; }
    }
}
