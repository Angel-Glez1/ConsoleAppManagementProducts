using AdmiProducts.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Identifier { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public Estatus EstatusId { get; set; }


        public User() { }

        public User(int userId, string identifier, string name, Estatus estatusId)
        {
            UserId = userId;
            Identifier = identifier;
            Name = name;
            EstatusId = estatusId;
        }
    }
}
