using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Exceptions
{
    public class BitacoraProductsException: Exception
    {

        public BitacoraProductsException(string? message) : base(message)
        {
        }
    }
}
