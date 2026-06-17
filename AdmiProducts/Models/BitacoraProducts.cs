using AdmiProducts.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Models
{
    public class BitacoraProducts
    {
        public int BitacoraLogId { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string ProductDescription { get; set; } = String.Empty;
        public Actions ActionID { get; set; }
        public DateTime ExecutionDate { get; set; }
    }
}
