using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Transport
{
    public class CreateTransportRequest
    {
        public int order_id { get; set; }
        public int transporter_id { get; set; }
        public decimal cod_price { get; set; }
        public string lading_code { get; set; }
    }
}
