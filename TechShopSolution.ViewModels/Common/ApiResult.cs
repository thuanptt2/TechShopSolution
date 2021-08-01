using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Common
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? statusCode { get; set; }
        public T ResultObject { get; set; }
    }
}
