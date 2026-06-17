using AdmiProducts.Controllers;
using AdmiProducts.Repositories;
using AdmiProducts.Repositories.Interfaces;
using AdmiProducts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// 1. Cargar configuración desde appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
    .Build();

var connectionString = config.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión.");

// 2. Configurar el contenedor de DI
var services = new ServiceCollection();

// 3. Registrar repositorios, servicios y controller
services.AddTransient<IProductRepository>(_ => new ProductRepository(connectionString));
services.AddTransient<IUserRepository>(_ => new UserRepository(connectionString));
services.AddTransient<IBitacoraProductsRepository>(_ => new BitacoraProductsRepository(connectionString));
services.AddTransient<ProductService>();
services.AddTransient<UserService>();
services.AddTransient<BitacoraProductService>();
services.AddTransient<MenuController>();

// 4. Construir el contenedor con validación activa
using ServiceProvider provider = services.BuildServiceProvider(
    new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }
);

// 5. Resolver y ejecutar
var menuController = provider.GetRequiredService<MenuController>();
await menuController.Start();