using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Customer
{
    public class CustomerRegisterRequest
    {
        public string lastname { get; set; }
        public string firstname { get; set; }
        [Remote(action: "VerifyEmail", controller: "Account")]
        public string email { get; set; }
        public string password { get; set; }
        [Compare(nameof(password), ErrorMessage = "Mật khẩu không trùng khớp")]
        public string confirmpassword { get; set; }
        public string phone { get; set; }
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        public bool sex { get; set; }
    }
}
