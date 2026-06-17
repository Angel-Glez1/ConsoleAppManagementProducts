using AdmiProducts.Exceptions;
using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Services;
using AdmiProducts.Utils;

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
            ConsoleUI.ShowHeader("BIENVENIDO");

            bool successfulLogin = await Login();
            if (!successfulLogin || _loggedUser is null) return;

            bool showMenu = true;

            while (showMenu)
            {
                try
                {
                    ConsoleUI.ShowMenu();
                    ConsoleUI.MsgUser($"Usuario: {_loggedUser.Name}");
                    int option = ConsoleUI.ReadInt("Selecciona una opción: ");

                    switch (option)
                    {
                        case 1:
                            ConsoleUI.Clear();
                            await ListarProductos();
                            ConsoleUI.Pause();
                            break;

                        case 2:
                            await CrearNuevoProducto(_loggedUser);
                            ConsoleUI.Pause();
                            break;

                        case 3:
                            await EditarProducto(_loggedUser);
                            ConsoleUI.Pause();
                            break;

                        case 4:
                            await EliminarProducto(_loggedUser);
                            ConsoleUI.Pause();
                            break;

                        case 5:
                            await MostrarBitacora(_loggedUser);
                            ConsoleUI.Pause();
                            break;

                        case 6:
                            showMenu = false;
                            ConsoleUI.Clear();
                            ConsoleUI.MsgUser($"Adiós: {_loggedUser.Name}");
                            break;

                        default:
                            ConsoleUI.Error($"El número: {option}, no es una opción válida.");
                            ConsoleUI.Pause();
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
                    ConsoleUI.Error("Excepción no controlada. Comuníquese con el administrador... " + ex.Message);
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

        private async Task CrearNuevoProducto(User loggedUser)
        {
            ConsoleUI.Clear();

            string description = ConsoleUI.ReadText("Escribe el nombre del producto: ");
            int quantity = ConsoleUI.ReadInt("Escribe la cantidad de stock del producto: ");

            int newProductId = await _productService.CreateAsync(description, quantity);
            ConsoleUI.Success("Producto creado con éxito.");

            try
            {
                await _bitacoraProductService.AddLogAsync(loggedUser.UserId, newProductId, Actions.Agregar);
            }
            catch (BitacoraProductsException ex)
            {
                ConsoleUI.Info($"El producto se creó, pero no se pudo registrar en la bitácora: {ex.Message}");
            }
        }

        private async Task EditarProducto(User loggedUser)
        {
            ConsoleUI.Clear();
            ConsoleUI.ShowHeader("Productos que puedes editar");
            await ListarProductos();

            ConsoleUI.Text("----------------------------------------------------------");
            int productId = ConsoleUI.ReadInt("\nEscribe el id del producto que quieres editar: ");

            var productoActual = await _productService.FindByIdAsync(productId);
            if (productoActual is null)
            {
                ConsoleUI.Error($"No existe un producto con el id: {productId}");
                return;
            }

            string description = ConsoleUI.ReadText("Escribe el nuevo nombre del producto: ");
            int quantity = ConsoleUI.ReadInt("Escribe la nueva cantidad de stock del producto: ");

            var productoEditado = new Product(productId, description, quantity, productoActual.EstatusId);
            await _productService.UpdateAsync(productoEditado);
            ConsoleUI.Success("Producto actualizado con éxito.");

            try
            {
                await _bitacoraProductService.AddLogAsync(loggedUser.UserId, productId, Actions.Editar);
            }
            catch (BitacoraProductsException ex)
            {
                ConsoleUI.Info($"El producto se actualizó, pero no se pudo registrar en la bitácora: {ex.Message}");
            }
        }

        private async Task EliminarProducto(User loggedUser)
        {
            ConsoleUI.Clear();
            ConsoleUI.ShowHeader("Productos que puedes eliminar");
            await ListarProductos();

            ConsoleUI.Text("----------------------------------------------------------");
            int productId = ConsoleUI.ReadInt("\nEscribe el id del producto a eliminar: ");

            string confirm = ConsoleUI.ReadText("¿Estás seguro de realizar esta acción? (y/n): ");
            if (confirm.ToLower() == "n")
            {
                ConsoleUI.Info("Proceso cancelado.");
                return;
            }

            await _productService.DeleteAsync(productId);
            ConsoleUI.Success("Producto eliminado correctamente.");

            try
            {
                await _bitacoraProductService.AddLogAsync(loggedUser.UserId, productId, Actions.Eliminar);
            }
            catch (BitacoraProductsException ex)
            {
                ConsoleUI.Info($"El producto se eliminó, pero no se pudo registrar en la bitácora: {ex.Message}");
            }
        }

        private async Task MostrarBitacora(User loggedUser)
        {
            ConsoleUI.Clear();

            var logs = await _bitacoraProductService.GetLogByUserIdAsync(loggedUser.UserId);

            ConsoleUI.ShowTable(logs,
                ("LOG ID", l => l.BitacoraLogId),
                ("Usuario", l => l.UserName),
                ("Producto", l => l.ProductDescription),
                ("Acción", l => l.ActionID),
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
                    var user = await _userService.Login(identifier);
                    _loggedUser = user;
                    return true;
                }
                catch (BusinessException ex)
                {
                    if (ex.ErrorCode == BusinessExceptionErrorCode.CuentaBloqueda)
                    {
                        ConsoleUI.Clear();
                        ConsoleUI.Error(ex.Message);
                        return false;
                    }

                    count++;
                    ConsoleUI.Error($"Intento {count}/{LIMIT_LOGIN_TRY}: {ex.Message}");
                }
            }

            ConsoleUI.Clear();
            ConsoleUI.Error("Excediste el número de intentos permitidos.");
            return false;
        }
    }
}