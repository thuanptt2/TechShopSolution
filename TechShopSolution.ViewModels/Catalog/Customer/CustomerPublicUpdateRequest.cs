using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Customer
{
    public class CustomerPublicUpdateRequest
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        public bool sex { get; set; }
    }
}
