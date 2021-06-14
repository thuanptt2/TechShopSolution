using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Catalog.Location;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : Controller
    {

        private readonly ILoadLocationService _loadLocationService;

        public LocationController(ILoadLocationService loadLocationService)
        {
            _loadLocationService = loadLocationService;
        }
        [HttpGet("LoadProvince")]
        public IActionResult LoadProvince()
        {
            var lstProvince = _loadLocationService.LoadProvince();
            if (lstProvince != null)
            {
                return Ok(lstProvince);
            }
            return null;
        }
        [HttpGet("LoadDistrict")]
        public IActionResult LoadDistrict(int provinceID)
        {
            var lstProvince = _loadLocationService.LoadDistrict(provinceID);
            if (lstProvince != null)
            {
                return Ok(lstProvince);
            }
            return null;
        }
        [HttpGet("LoadWard")]
        public IActionResult LoadWard(int districtID)
        {
            var lstProvince = _loadLocationService.LoadWard(districtID);
            if (lstProvince != null)
            {
                return Ok(lstProvince);
            }
            return null;
        }
    }
}
