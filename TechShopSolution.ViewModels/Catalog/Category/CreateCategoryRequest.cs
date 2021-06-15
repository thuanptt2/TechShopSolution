using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Category
{
    public class CreateCategoryRequest
    {
        [Display(Name = "Tên loại sản phẩm")]
        public string cate_name { get; set; }
        [Display(Name = "Đường dẫn")]
        [Remote(action: "isValidSlug", controller: "Category")]
        public string cate_slug { get; set; }
        [Display(Name = "Loại sản phẩm cha")]
        public int? parent_id { get; set; }
        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }
        [Display(Name = "Tiêu đề trang")]
        public string meta_title { get; set; }
        [Display(Name = "Từ khóa trang")]
        public string meta_keywords { get; set; }
        [Display(Name = "Mô tả trang")]
        public string meta_descriptions { get; set; }
    }
}
