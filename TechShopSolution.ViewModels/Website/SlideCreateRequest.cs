using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Website
{
    public class SlideCreateRequest
    {
        public IFormFile image { get; set; }
        public int display_order { get; set; }
        public string link { get; set; }
        public bool status { get; set; }
    }
}
