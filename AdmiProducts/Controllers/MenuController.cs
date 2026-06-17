using AdmiProducts.Exceptions;
using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
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
        private readonly BitacoraProductService _bitacoraProductService;
        private User? _loggedUser = null;

        public MenuController(ProductService productService, UserService userService, BitacoraProductService bitacoraProductService)
        {
            _productService = productService;
            _userService = userService;
            _bitacoraProductService = bitacoraProductService;
        }



        public async Task Start()
        {
            //1. Mensaje bienvenida.
            ConsoleUI.ShowHeader("BIENVENIDO");

            //2. Login
            bool successfulLogin = await Login();
            if (!successfulLogin || _loggedUser is null) return;


            //3. Mostrar menu principal
            bool showMenu = true;

            // Mostar menu principal hasta que el usuario quiera salir.
            while (showMenu)
            {
                try
                {
                    ConsoleUI.ShowMenu();
                    ConsoleUI.MsgUser($"Usuario: {_loggedUser?.Name}");
                    int option = ConsoleUI.ReadInt("Selecciona una opción: ");


                    switch (option)
                    {

                        case 1:
                            ConsoleUI.Clear();
                            await ListarProductos();
                            ConsoleUI.Pause();
                            break;

                        case 2:
                            await CrearNuevoProducto();
                            ConsoleUI.Pause();
                            break;

                        case 3:
                            await EditarProducto();
                            ConsoleUI.Pause();
                            break;

                        case 4:
                            await EliminarProducto();
                            ConsoleUI.Pause();
                            break;

                        case 5:
                            await GetUserBitacora();
                            ConsoleUI.Pause();
                            break;


                        case 6:
                            showMenu = false;
                            ConsoleUI.Clear();
                            ConsoleUI.MsgUser($"Adios: {_loggedUser?.Name}");
                            break;

                        default:
                            //ConsoleUI.Clear();
                            ConsoleUI.Error($"El número: {option}, no es una opción valida.");
                            ConsoleUI.Pause(); // Al pausar, le permitimos al sistema poder mostar los mensajes.
                            break;
                    }

                }
                catch (BusinessException ex)
                {
                    ConsoleUI.Error(ex.Message);
                    ConsoleUI.Pause();
                }
                catch (BitacoraProductsException ex)
                {
                    ConsoleUI.Info(ex.Message);
                    ConsoleUI.Pause();
                }
                catch (Exception ex)
                {
                    ConsoleUI.Error("Exepción no controlada. Comuniquese con el Administrados..." + ex.Message.ToString());
                    ConsoleUI.Pause();
                }
            }


        }


        private async Task ListarProductos()
        {
            List<Product> products = await _productService.FindAllAsync();

            ConsoleUI.ShowTable(products,
                ("ID", p => p.ProductId),
                ("Descripción", p => p.Description),
                ("Cantidad", p => p.Quantity),
                ("Estatus", p => p.EstatusId)
            );
        }


        private async Task CrearNuevoProducto()
        {
            ConsoleUI.Clear();


            // Solicita los datos del nuevo producto, al usuario.
            string description = ConsoleUI.ReadText("Escribe el nombre del producto: ");
            int quantity = ConsoleUI.ReadInt("Escribe la cantidad de stock del producto: ");


            // Ejecuta el servicio para crear un nuevo producto y muestra mensaje de exito
            int newProductId = await _productService.CreateAsync(description, quantity);
            ConsoleUI.Success($"Producto creado con existo");

            // Guardamos en bitacora el movimiento
            await _bitacoraProductService.AddLogAsync(_loggedUser!.UserId, newProductId, Actions.Agregar);
        }


        private async Task EditarProducto()
        {
            ConsoleUI.Clear();

            // Lista el catálogo de productos que se pueden editar.
            ConsoleUI.ShowHeader("Productos que puedes editar");
            await ListarProductos();


            // Solicitamos los nuevos datos
            ConsoleUI.Text("----------------------------------------------------------");
            int productId = ConsoleUI.ReadInt("\nEscribe el id del producto que quieres editar: ");

            string description = ConsoleUI.ReadText("Escribe el nuevo nombre del producto: ");
            int quantity = ConsoleUI.ReadInt("Escribe la nueva cantidad de stock del producto: ");

            // Ejecuta el servicio para crear un nuevo producto y muestra mensaje de exito
            int newProductId = await _productService.UpdateAsync(new Product(productId, description, quantity, Estatus.Activo));
            ConsoleUI.Success($"Producto creado con existo");

            // Guardamos en bitacora el movimiento
            await _bitacoraProductService.AddLogAsync(_loggedUser!.UserId, productId, Actions.Editar);
        }


        private async Task EliminarProducto()
        {
            ConsoleUI.Clear();

            // Lista el catálogo de productos que se pueden eliminar.
            ConsoleUI.ShowHeader("Productos que puedes eliminar");
            await ListarProductos();

            // Solicita el id del producto a eliminar.
            ConsoleUI.Text("----------------------------------------------------------");
            int productId = ConsoleUI.ReadInt("\nEscribe el id del producto a eliminar: ");


            // Solicita confirmación para la eliminación
            string confirm = ConsoleUI.ReadText("¿Estas seguro de realizar esta acción? (y/n): ");
            if (confirm.ToLower() == "n")
            {
                ConsoleUI.Info($"Processo cancelado...");
                return;
            }


            // Ejecuta el servicio que elimina los premios
            await _productService.DeleteAsync(productId);
            ConsoleUI.Success($"Producto eliminado correctamente");


            // Guardamos en bitacora el movimiento
            await _bitacoraProductService.AddLogAsync(_loggedUser!.UserId, productId, Actions.Eliminar);
        }


        private async Task GetUserBitacora()
        {
            ConsoleUI.Clear();

            var logs = await _bitacoraProductService.GetLogByUserIdAsync(_loggedUser!.UserId);

            ConsoleUI.ShowTable(logs,
                ("LOG ID", l => l.BitacoraLogId),
                ("User", l => l.UserName),
                ("Producto", l => l.ProductDescription),
                ("Action", l => l.ActionID),
                ("Fecha", l => l.ExecutionDate)
            );
        }



        private async Task<bool> Login()
        {
            const int LIMIT_LOGIN_TRY = 3;
            int count = 0;

            while (count < LIMIT_LOGIN_TRY)
            {
                string identifier = ConsoleUI.ReadText("Ingresa tu usuario: ");

                try
                {
                    // Si el login es exitoso, salimos de inmediato
                    var user = await _userService.Login(identifier);
                    _loggedUser = user;
                    return true;
                }
                catch (BusinessException ex)
                {
                    ConsoleUI.Clear();

                    // La cuenta esta bloqueada. Salir del while.
                    if (ex.ErrorCode == BusinessExceptionErrorCode.CuentaBloqueda)
                    {
                        ConsoleUI.Error(ex.Message);
                        return false;
                    }


                    // Mostar error de intentos fallidos ( al 3 se bloquea )
                    count++;
                    ConsoleUI.Error($"Intento {count}/{LIMIT_LOGIN_TRY}: {ex.Message}");
                }
            }

            // Si llegamos aquí, se agotaron los intentos
            ConsoleUI.Clear();
            ConsoleUI.Error("Excediste el número de intentos permitidos.");
            return false;
        }
    }
}