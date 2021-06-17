using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Location;

namespace TechShopSolution.Application.Catalog.Location
{
    public class LoadLocationService : ILoadLocationService
    {

        private IWebHostEnvironment _hostEnvironment;

        public LoadLocationService(IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
        }
        private readonly XDocument xmlDoc = XDocument.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/Provinces_Data.xml"));

        public ApiResult<List<ProvinceModel>> LoadProvince()
        {
            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            if (xElements == null)
                return new ApiErrorResult<List<ProvinceModel>>("Không đọc được dữ liệu");
            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);
            }
            return new ApiSuccessResult<List<ProvinceModel>>(list);
        }
        public ApiResult<List<DistrictModel>> LoadDistrict(int provinceID)
        {
            var xElement = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);
            if (xElement == null)
                return new ApiErrorResult<List<DistrictModel>>("Không đọc được dữ liệu");
            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                list.Add(district);
            }
            return new ApiSuccessResult<List<DistrictModel>>(list);
        }

        public ApiResult<List<WardModel>> LoadWard(int districtID)
        {
            var xElement = xmlDoc.Element("Root").Elements("Item").Elements("Item")
               .Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == districtID);
            if (xElement == null)
                return new ApiErrorResult<List<WardModel>>("Không đọc được dữ liệu");
            var list = new List<WardModel>();
            WardModel ward = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "precinct"))
            {
                ward = new WardModel();
                ward.ID = int.Parse(item.Attribute("id").Value);
                ward.Name = item.Attribute("value").Value;
                ward.DistrictId = int.Parse(xElement.Attribute("id").Value);
                list.Add(ward);
            }
            return new ApiSuccessResult<List<WardModel>>(list);
        }
    }
}
