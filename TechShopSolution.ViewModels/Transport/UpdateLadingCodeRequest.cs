using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Transport
{
    public class UpdateLadingCodeRequest
    {
        public int Id { get; set; }
        [Display(Name = "Mã vận đơn mới")]
        [Required(ErrorMessage = "Vui lòng điền mã vận đơn")]
        public string New_LadingCode { get; set; }
    }
}
