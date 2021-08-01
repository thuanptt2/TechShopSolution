using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public ApiErrorResult(string message, int? statuscode = null)
        {
            IsSuccess = false;
            Message = message;
            statusCode = statuscode;
        }
    }
}
