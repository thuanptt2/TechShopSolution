using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Product
{
    public class ProductCreateRequest
    {
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public string Code { get; set; }
        [Display(Name = "Đường dẫn sản phẩm")]
        [Remote(action: "isValidSlug", controller: "Product")]
        public string Slug { get; set; }
        [Display(Name = "Thương hiệu")]
        public int Brand_id { get; set; }
        [Display(Name = "Danh mục sản phẩm")]
        public string CateID { get; set; }
        [Display(Name = "Hình ảnh")]
        public IFormFile Image { get; set; }
        [Display(Name = "Hình ảnh chi tiết")]
        public List<IFormFile> More_images { get; set; }
        [Display(Name = "Đơn giá")]
        public decimal Unit_price { get; set; }
        [Display(Name = "Giá khuyến mãi")]
        public decimal Promotion_price { get; set; }
        [Display(Name = "Bảo hành")]
        public int Warranty { get; set; }
        [Display(Name = "Trong kho")]
        public int? Instock { get; set; }
        [Display(Name = "Thông số kỹ thuật")]
        public string Specifications { get; set; }
        [Display(Name = "Mô tả ngắn")]
        public string Short_desc { get; set; }
        [Display(Name = "Chi tiết sản phẩm")]
        public string Descriptions { get; set; }
        [Display(Name = "Sản phẩm nổi bật")]
        public bool Featured { get; set; }
        [Display(Name = "Sản phẩm bán chạy")]
        public bool Best_seller { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }
        [Display(Name = "Tiêu đề trang")]
        public string Meta_tittle { get; set; }
        [Display(Name = "Từ khóa trang")]
        public string Meta_keywords { get; set; }
        [Display(Name = "Miêu tả trang")]
        public string Meta_descriptions { get; set; }
    }
}
