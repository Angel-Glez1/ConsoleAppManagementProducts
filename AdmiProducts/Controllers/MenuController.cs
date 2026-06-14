using AdmiProducts.Exceptions;
using AdmiProducts.Models;
using AdmiProducts.Services;
using AdmiProducts.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Controllers
{
    public class MenuController
    {

        private readonly ProductService _productService;
        private readonly UserService _userService;

        public MenuController(ProductService productService, UserService userService)
        {
            _productService = productService;
            _userService = userService;
        }



        public async Task Start()
        {
            try
            {
                //1. Mensaje bienvenida.
                ConsoleUI.ShowHeader("BIENVENIDO");

                //2. Login
                User user = await Login();

                
            }
            catch (Exception ex)
            {
                ConsoleUI.Error(ex.Message);
            }

        }



        public async Task<User> Login()
        {
            const int LIMIT_LOGIN_TRY = 3;
            int count = 0;

            while (count < LIMIT_LOGIN_TRY)
            {
                string identifier = ConsoleUI.ReadText("Ingresa tu usuario: ");

                try
                {
                    // Si el login es exitoso, salimos de inmediato
                    return await _userService.Login(identifier);
                }
                catch (BusinessException ex)
                {

                    // La cuenta esta bloqueada. Salir del while.
                    if (ex.ErrorCode == BusinessExceptionErrorCode.CuentaBloqueda) throw;

                    count++;
                    ConsoleUI.Error($"Intento {count}/{LIMIT_LOGIN_TRY}: {ex.Message}");
                }
            }

            // Si llegamos aquí, se agotaron los intentos
            throw new BusinessException("Excediste el número de intentos permitidos.");

        }
    }
}