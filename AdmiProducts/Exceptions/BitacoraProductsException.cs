using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Exceptions
{
    public class BitacoraProductsException : Exception
    {
        public BitacoraProductsException(string? message) : base(message) { }

        public BitacoraProductsException(string? message, Exception innerException)
            : base(message, innerException) { }
    }
}
