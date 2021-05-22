using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Data.Entities
{
    public class Transporter
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
        public List<Transport> Transports { get; set; }
    }
}
