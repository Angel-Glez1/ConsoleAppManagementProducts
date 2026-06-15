using AdmiProducts.Exceptions;
using AdmiProducts.Models;
using AdmiProducts.Services;
using AdmiProducts.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

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

                //3. Mostrar menu principal
                bool showMenu = true;

                while (showMenu)
                {

                    ConsoleUI.ShowMenu();
                    ConsoleUI.MsgUser($"Usuario: {user.Name}");
                    int option = ConsoleUI.ReadInt("Selecciona una opción: ");


                    switch (option)
                    {

                        case 1:
                            // Consultar productos
                            break;

                        case 2:
                            // Insertar
                            break;

                        case 3:
                            // Actualizar
                            break;

                        case 4:
                            // Eliminar
                            break;

                        case 5:
                            showMenu = false;
                            ConsoleUI.MsgUser($"Usuario: {user.Name}");
                            break;

                        default:
                            ConsoleUI.Error($"El número: {option}, no es una opción valida.");
                            Thread.Sleep(2000);
                            break;
                    }
                }


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
                    if (ex.ErrorCode == BusinessExceptionErrorCode.CuentaBloqueda)
                        throw;

                    count++;
                    ConsoleUI.Error($"Intento {count}/{LIMIT_LOGIN_TRY}: {ex.Message}");
                }
            }

            // Si llegamos aquí, se agotaron los intentos
            throw new BusinessException("Excediste el número de intentos permitidos.");

        }
    }
}