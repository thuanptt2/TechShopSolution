using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Customer
{
    public class CustomerUpdateAddressRequest
    {
        public int Id { get; set; }
        [Display(Name = "Tỉnh/Thành phố")]
        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành")]
        public string City { get; set; }
        [Display(Name = "Quận/Huyện")]
        [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện")]
        public string District { get; set; }
        [Display(Name = "Phường/Xã")]
        [Required(ErrorMessage = "Vui lòng chọn Phường/Xã")]
        public string Ward { get; set; }
        [Display(Name = "Số nhà, tên đường")]
        public string House { get; set; }
    }
}
