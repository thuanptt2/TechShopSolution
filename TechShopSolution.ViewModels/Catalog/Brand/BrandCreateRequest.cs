using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Brand
{
    public class BrandCreateRequest
    {
        [Display(Name ="Tên thương hiệu")]
        public string brand_name { get; set; }
        [Display(Name = "Đường dẫn thương hiệu")]
        [Remote(action: "isValidSlug", controller: "Brand")]
        public string brand_slug { get; set; }
        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }
        [Display(Name = "Tiêu đề thương hiệu")]
        public string meta_title { get; set; }
        [Display(Name = "Từ khóa thương hiệu")]
        public string meta_keywords { get; set; }
        [Display(Name = "Mô tả thương hiệu")]
        public string meta_descriptions { get; set; }
    }
}
