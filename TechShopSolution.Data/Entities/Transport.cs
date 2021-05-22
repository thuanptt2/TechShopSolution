using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Transport
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int transporter_id { get; set; }
        public decimal cod_price { get; set; }
        public string lading_code { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
    }
}
