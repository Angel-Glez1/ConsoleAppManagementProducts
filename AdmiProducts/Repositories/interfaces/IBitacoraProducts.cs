using AdmiProducts.Models;
using AdmiProducts.Models.Enums;

namespace AdmiProducts.Repositories.Interfaces
{
    public interface IBitacoraProductsRepository
    {
        Task InsertAsync(int userId, int productId, Actions actionId);
        Task<List<BitacoraProducts>> GetAsync(int userId);
    }
}