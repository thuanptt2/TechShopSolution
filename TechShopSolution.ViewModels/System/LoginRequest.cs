using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.System
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remeber_me { get; set; }
    }
}
