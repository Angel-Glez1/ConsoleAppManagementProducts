using AdmiProducts.Models.Enums;

namespace AdmiProducts.Models
{
    public class BitacoraProducts
    {
        public int BitacoraLogId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public Actions ActionID { get; set; }
        public DateTime ExecutionDate { get; set; }
    }
}