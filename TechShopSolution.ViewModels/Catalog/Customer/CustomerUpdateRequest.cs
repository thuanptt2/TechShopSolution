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
        public string name { get; set; }
        [Display(Name = "Số điện thoại")]
        public string phone { get; set; }
        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        [Display(Name = "Địa chỉ Email")]
        public string email { get; set; }
        [Display(Name = "Giới tính")]
        public bool sex { get; set; }
        public string address { get; set; }
    }
}
