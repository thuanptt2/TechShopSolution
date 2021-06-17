using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Location;

namespace TechShopSolution.Application.Catalog.Location
{
    public interface ILoadLocationService
    {
        ApiResult<List<ProvinceModel>> LoadProvince();
        ApiResult<List<DistrictModel>> LoadDistrict(int provinceID);
        ApiResult<List<WardModel>> LoadWard(int districtID);
    }
}
