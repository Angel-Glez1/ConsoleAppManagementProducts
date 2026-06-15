using AdmiProducts.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Repositories.Interfaces
{
    public interface IBitacoraProducts
    {
        Task<int> Insert(int userId, int productId, Actions actionId);
    }
}
