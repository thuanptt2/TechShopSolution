using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Utilities.Exceptions
{
    public class TechshopException : Exception
    {
        public TechshopException()
        {
        }

        public TechshopException(string message)
            : base(message)
        {
        }

        public TechshopException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
