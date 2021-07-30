using System.Collections.Generic;
using TechShopSolution.ViewModels.Website.Dashboard;

namespace TechShopSolution.AdminApp.Models
{
    public class DashBoardViewModel
    {
        public OrderStatisticsViewModel OrderStatistics { get; set; }
        public List<ProductRankingViewModel> viewRanking { get; set; }
        public List<ProductRankingViewModel> salesRanking { get; set; }
        public List<ProductRankingViewModel> favoriteRanking { get; set; }
    }
}
