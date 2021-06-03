using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public ApiErrorResult(string message)
        {
            IsSuccess = false;
            Message = message;
        }
    }
}
