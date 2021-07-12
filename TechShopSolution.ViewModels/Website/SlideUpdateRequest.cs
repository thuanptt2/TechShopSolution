using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Website
{
    public class SlideUpdateRequest
    {
        [Display(Name = "Hình ảnh Slide")]
        public int id { get; set; }
        public int display_order { get; set; }
        public IFormFile image { get; set; }
        [Display(Name = "Link liên kết")]
        public string link { get; set; }
        [Display(Name = "Trạng thái")]
        public bool status { get; set; }
        public string imageBase64 { get; set; }
    }
}
