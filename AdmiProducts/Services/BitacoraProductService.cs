using AdmiProducts.Exceptions;
using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;

namespace AdmiProducts.Services
{
    public class BitacoraProductService
    {
        private readonly IBitacoraProductsRepository _bitacoraProductsRepository;

        public BitacoraProductService(IBitacoraProductsRepository bitacoraProductsRepository)
        {
            _bitacoraProductsRepository = bitacoraProductsRepository;
        }

        public async Task AddLogAsync(int userId, int productId, Actions actionId)
        {
            try
            {
                await _bitacoraProductsRepository.InsertAsync(userId, productId, actionId);
            }
            catch (Exception ex)
            {
                throw new BitacoraProductsException("Fallo al registrar el log de la bitácora.", ex);
            }
        }

        public async Task<List<BitacoraProducts>> GetLogByUserIdAsync(int userId)
        {
            return await _bitacoraProductsRepository.GetAsync(userId);
        }
    }
}