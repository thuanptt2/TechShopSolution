using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T resultObj)
        {
            IsSuccess = true;
            ResultObject = resultObj;
        }
        public ApiSuccessResult()
        {
            IsSuccess = true;
        }
    }
}
