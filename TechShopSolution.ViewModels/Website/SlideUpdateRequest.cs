using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Website
{
    public class SlideUpdateRequest
    {
        public int id { get; set; }
        public IFormFile image { get; set; }
        public int display_order { get; set; }
        public string link { get; set; }
        public bool status { get; set; }
        public string imageBase64 { get; set; }
    }
}
