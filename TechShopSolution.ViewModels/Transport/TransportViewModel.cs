using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Transport
{
    public class TransportViewModel
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int transporter_id { get; set; }
        public string transporter_name { get; set; }
        public int ship_status { get; set; }
        public string from_address { get; set; }
        public string to_address { get; set; }
        public decimal cod_price { get; set; }
        public string lading_code { get; set; }
        public DateTime create_at { get; set; }
        public DateTime? update_at { get; set; }
    }
}
