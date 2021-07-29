using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Sales
{
    public class OrderStatisticsViewModel
    {
        public int orderWaitForConfirm { get; set; }
        public int orderWaitForPay { get; set; }
        public int orderWaitForShip { get; set; }
        public int orderBeingShip { get; set; }
    }
}
