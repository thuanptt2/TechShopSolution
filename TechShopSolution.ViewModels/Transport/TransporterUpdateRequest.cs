using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Transport
{
    public class TransporterUpdateRequest
    {
        public int id { get; set; }
        [Display(Name = "Tên đơn vị")]
        public string name { get; set; }
        [Display(Name = "Hình ảnh")]
        public IFormFile image { get; set; }
        [Display(Name = "Link tra cứu")]
        public string link { get; set; }
        public string imageBase64 { get; set; }
        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }
    }
}
