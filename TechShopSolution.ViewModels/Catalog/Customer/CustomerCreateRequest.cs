using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TechShopSolution.ViewModels.Catalog.Customer
{
    public class CustomerCreateRequest
    {
        
        [Display(Name ="Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string name { get; set; }
        [Remote(action: "VerifyEmail", controller: "Customer")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email")]
        [Display(Name = "Địa chỉ Email")]
        public string email { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string password { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage ="Vui lòng nhập đúng định dạng")]
        public string phone { get; set; }
        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        [Display(Name = "Giới tính")]
        public bool sex { get; set; }
        [Display(Name = "Tỉnh/Thành phố")]
        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành")]
        public string City { get; set; }
        [Display(Name = "Quận/Huyện")]
        [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện")]
        public string District { get; set; }
        [Display(Name = "Phường/Xã")]
        [Required(ErrorMessage = "Vui lòng chọn Phường/Xã")]
        public string Ward { get; set; }
        [Display(Name = "Số nhà, tên đường")]
        public string House { get; set; }
        [Display(Name = "Trạng thái")]
        public bool status { get; set; }
    }
}
