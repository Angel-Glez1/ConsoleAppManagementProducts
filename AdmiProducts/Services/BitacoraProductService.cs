using AdmiProducts.Exceptions;
using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
                await _bitacoraProductsRepository.Insert(userId, productId, actionId);

            }
            catch (Exception ex)
            {
                throw new BitacoraProductsException("Fallo al registar el log de la bitacora  | " + ex.Message);
            }
        }



        public async Task<List<BitacoraProducts>> GetLogByUserIdAsync(int userId)
        {
            return await _bitacoraProductsRepository.Get(userId);
        }

    }
}
