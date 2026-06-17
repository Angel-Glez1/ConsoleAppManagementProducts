using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Repositories.Interfaces
{
    public interface IBitacoraProductsRepository
    {
        Task<int> Insert(int userId, int productId, Actions actionId);

        Task<List<BitacoraProducts>> Get(int userId);

    }
}
