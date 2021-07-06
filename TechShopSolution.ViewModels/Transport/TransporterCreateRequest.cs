using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Transport
{
    public class TransporterCreateRequest
    {
        public string name { get; set; }
        public IFormFile image { get; set; }
        public string link { get; set; }
        public bool isActive { get; set; }
    }
}
