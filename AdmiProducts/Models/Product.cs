using AdmiProducts.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Description { get; set; } = String.Empty;
        public int Quantity { get; set; }
        public Estatus EstatusId { get; set; }


        public Product(int productId, string description, int quantity, Estatus estatusId) 
        { 
            ProductId = productId;
            Description = description;
            Quantity = quantity;
            EstatusId = estatusId;
        }

    }
}
