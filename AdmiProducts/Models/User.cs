using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Models
{
    public class User
    {
        public int userId { get; set; }
        public string identifier { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public int estatusId { get; set; }
    }
}
