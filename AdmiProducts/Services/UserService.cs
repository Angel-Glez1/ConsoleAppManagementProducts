using AdmiProducts.Exceptions;
using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        public async Task<User> Login(string identifier)
        {

            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new BusinessException("El usuario no puede estar vacío.");
            }


            var user = await _userRepository.GetUserByIdentifier(identifier);

            if (user is null)
            {
                throw new BusinessException("No existe esa cuenta.");
            }


            if (user.EstatusId == Estatus.Baja)
            {
                throw new BusinessException(
                    "Tu cuenta está bloqueada. Contacta al administrador.",
                    errorCode: BusinessExceptionErrorCode.CuentaBloqueda
                );

            }

            return user;
        }



    }
}
