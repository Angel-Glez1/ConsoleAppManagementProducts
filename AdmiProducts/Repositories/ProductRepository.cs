using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
                ORDER BY 1 DESC
            ";


            // 1. Crear objeto para la conxion.
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();


            // 2. Ejecutamos la query y obtenemos resultados
            using var command = new SqlCommand(query, connection);
            using var result = await command.ExecuteReaderAsync();


            // 3. Leer resultado de la query, obtener filas y agregar a la lista de productos un nuevo producto.
            while (await result.ReadAsync())
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

        public async Task<Product?> FindById(int id)
        {
            Product? _product = null;
            string query = @"
                 SELECT 
                     productId
                    ,description
                    ,Quantity
                    ,estatusId 
                FROM Products WHERE estatusId = 1
                AND productId = @id
            ";

            // Preparar la conexión y la abré.
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();


            // Prepera la query para su ejecución
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            // Ejecuta la query en base de datos.
            using var reader = await command.ExecuteReaderAsync();

            bool existsProduct = reader.HasRows;
            if (!existsProduct)
                return _product;


            // Leer resultado
            while (await reader.ReadAsync())
            {
                int productId = reader.GetInt32(0);
                string description = reader.GetString(1);
                int quantity = reader.GetInt32(2);
                Estatus estatusId = (Estatus)reader.GetInt32(3);

                _product = new Product(productId, description, quantity, estatusId);
            }

            return _product;
        }


        public async Task<int> Update(Product product)
        {
            string query = @"
                UPDATE Products SET 
                     description = @description
                    ,quantity = @quantity
                WHERE productId = @id
            ";

            // Preprar objeto para conexión y abrirla.
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Crear un objeto para poder configurar la query.
            using var command = new SqlCommand(query, connection);

            // Remplaza valores para evitar Sql Inyectión.
            command.Parameters.AddWithValue("@description", product.Description);
            command.Parameters.AddWithValue("@quantity", product.Quantity);
            command.Parameters.AddWithValue("@id", product.ProductId);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected;
        }


        public async Task<int> Create(string description, int quantity)
        {
            string query = @"
             INSERT INTO Products (
                     [description]
                    ,[quantity]
                    ,[estatusId]
                )
                OUTPUT INSERTED.productId
                VALUES (
                    @description,
                    @quantity,
                    @estatusId
                )
            ";

            // Preprar objeto para conexión y abrirla.
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Crear un objeto para poder configurar la query.
            using var command = new SqlCommand(query, connection);

            // Remplaza valores para evitar Sql Inyectión.
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@estatusId", Estatus.Activo);

            var rowsAffected = await command.ExecuteScalarAsync();
            return Convert.ToInt32(rowsAffected);
        }


        public async Task<int> Delete(int id)
        {
            string query = @"
                UPDATE Products SET
                    estatusId = 2
                WHERE productId = @id
                
            ";

            // Preprar objeto para conexión y abrirla.
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Crear un objeto para poder configurar la query.
            using var command = new SqlCommand(query, connection);

            // Remplaza valores para evitar Sql Inyectión.
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected;
        }
    }
}
