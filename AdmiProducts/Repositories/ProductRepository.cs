using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace AdmiProducts.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;


        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<List<Product>> FindAll()
        {
            List<Product> products = new List<Product>();
            string query = @"
                SELECT 
                     productId
                    ,description
                    ,Quantity
                    ,estatusId 
                FROM Products WHERE estatusId = 1
            ";


            // 1. Crear objeto para la conxion.
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();


            // 2. Ejecutamos la query y obtenemos resultados
            using var command = new SqlCommand(query, connection);
            using var result =  await command.ExecuteReaderAsync();

            
            // 3. Leer resultado de la query, obtener filas y agregar a la lista de productos un nuevo producto.
            while ( await result.ReadAsync() )
            {
                int productId = result.GetInt32(0);
                string description = result.GetString(1);
                int quantity = result.GetInt32(2);
                Estatus estatusId = (Estatus)result.GetInt32(3);

                products.Add(new Product(productId, description, quantity, estatusId));
            }

            // Regresamos una lista de producto
            return products;
        }

        public Task<Product?> FindById(int id)
        {
            throw new NotImplementedException();
        }


        public Task Update(Product product)
        {
            throw new NotImplementedException();
        }


        public Task Create(Product product)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
