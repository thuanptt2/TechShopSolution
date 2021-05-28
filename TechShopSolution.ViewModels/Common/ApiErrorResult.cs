using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationError { get; set; }
        public ApiErrorResult( string message)
        {
            IsSuccess = false;
            Message = message;
        }
        public ApiErrorResult(string[] validationError)
        {
            IsSuccess = false;
            ValidationError = validationError;
        }
    }
}
