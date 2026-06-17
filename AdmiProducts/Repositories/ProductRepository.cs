using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace AdmiProducts.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Product>> FindAllAsync()
        {
            var products = new List<Product>();
            const string query = @"
                SELECT 
                     productId
                    ,description
                    ,Quantity
                    ,estatusId 
                FROM Products WHERE estatusId = 1
                ORDER BY 1 DESC
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int productId = reader.GetInt32(0);
                string description = reader.GetString(1);
                int quantity = reader.GetInt32(2);
                Estatus estatusId = (Estatus)reader.GetInt32(3);

                products.Add(new Product(productId, description, quantity, estatusId));
            }

            return products;
        }

        public async Task<Product?> FindByIdAsync(int id)
        {
            const string query = @"
                SELECT 
                     productId
                    ,description
                    ,Quantity
                    ,estatusId 
                FROM Products WHERE estatusId = 1
                AND productId = @id
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                int productId = reader.GetInt32(0);
                string description = reader.GetString(1);
                int quantity = reader.GetInt32(2);
                Estatus estatusId = (Estatus)reader.GetInt32(3);

                return new Product(productId, description, quantity, estatusId);
            }

            return null;
        }

        public async Task<int> UpdateAsync(Product product)
        {
            const string query = @"
                UPDATE Products SET 
                     description = @description
                    ,quantity = @quantity
                WHERE productId = @id
                AND estatusId = 1
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@description", product.Description);
            command.Parameters.AddWithValue("@quantity", product.Quantity);
            command.Parameters.AddWithValue("@id", product.ProductId);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> CreateAsync(string description, int quantity)
        {
            const string query = @"
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

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@estatusId", (int)Estatus.Activo);

            var newProductId = await command.ExecuteScalarAsync();
            return Convert.ToInt32(newProductId);
        }

        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"
                UPDATE Products SET
                    estatusId = 2
                WHERE productId = @id
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            return await command.ExecuteNonQueryAsync();
        }
    }
}