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
        public string name { get; set; }
        [Remote(action: "VerifyEmail", controller: "Customer")]
        [Display(Name = "Địa chỉ Email")]
        public string email { get; set; }
        [Display(Name = "Mật khẩu")]
        public string password { get; set; }
        [Display(Name = "Số điện thoại")]
        public string phone { get; set; }
        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        [Display(Name = "Giới tính")]
        public bool sex { get; set; }
        [Display(Name = "Tỉnh/Thành phố")]
        public string City { get; set; }
        [Display(Name = "Quận/Huyện")]
        public string District { get; set; }
        [Display(Name = "Phường/Xã")]
        public string Ward { get; set; }
        [Display(Name = "Số nhà, tên đường")]
        public string House { get; set; }
        [Display(Name = "Trạng thái")]
        public bool status { get; set; }

    }
}
