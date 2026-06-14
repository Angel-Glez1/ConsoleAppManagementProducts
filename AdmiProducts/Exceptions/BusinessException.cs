using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Exceptions
{

    public enum BusinessExceptionErrorCode
    {
        CuentaBloqueda = 0
    }

    /// <summary>
    /// Representa un error de negocio esperado que debe mostrarse al usuario.
    /// A diferencia de Exception, no indica un bug — indica una regla de negocio violada.
    /// </summary>
    public class BusinessException : Exception
    {


        public BusinessExceptionErrorCode ErrorCode { get; set; }

        public BusinessException(string message) : base(message) { }

        public BusinessException(string message, BusinessExceptionErrorCode errorCode) : base(message) { 
            ErrorCode = errorCode;
        }
    }
}
