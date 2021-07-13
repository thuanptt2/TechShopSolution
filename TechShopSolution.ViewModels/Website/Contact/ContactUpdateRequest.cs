using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Website.Contact
{
    public class ContactUpdateRequest
    {
        public int id { get; set; }
        [Display(Name = "Tên công ty")]
        public string company_name { get; set; }
        [Display(Name = "Địa chỉ")]
        public string adress { get; set; }
        [Display(Name = "Số điện thoại")]
        public string phone { get; set; }
        [Display(Name = "Đường dây nóng")]
        public string hotline { get; set; }
        [Display(Name = "Ảnh logo")]
        public IFormFile company_logo { get; set; }
        public string imageBase64 { get; set; }
        [Display(Name = "Số Fax")]
        public string fax { get; set; }
        [Display(Name = "Địa chỉ")]
        public string email { get; set; }
        [Display(Name = "Link facebook")]
        public string social_fb { get; set; }
        [Display(Name = "Link instargram")]
        public string social_instagram { get; set; }
        [Display(Name = "Link youtube")]
        public string social_youtube { get; set; }
        [Display(Name = "Link twitter")]
        public string social_twitter { get; set; }
    }
}
