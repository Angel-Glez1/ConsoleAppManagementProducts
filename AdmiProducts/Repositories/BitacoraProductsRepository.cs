using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace AdmiProducts.Repositories
{
    public class BitacoraProductsRepository : IBitacoraProductsRepository
    {
        private readonly string _connectionString;

        public BitacoraProductsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<BitacoraProducts>> GetAsync(int userId)
        {
            const string query = @"
                SELECT 
                     bitacoraId
                    ,Users.name
                    ,Products.description
                    ,Actions.actionId
                    ,executionDate
                FROM BitacoraProducts
                JOIN Users ON Users.userId = BitacoraProducts.userId
                JOIN Products ON Products.productId = BitacoraProducts.productId
                JOIN Actions ON Actions.actionId = BitacoraProducts.actionId
                WHERE Users.userId = @userId
                ORDER BY 1 DESC
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);

            using var reader = await command.ExecuteReaderAsync();

            var bitacoraProducts = new List<BitacoraProducts>();

            while (await reader.ReadAsync())
            {
                int bitacoraId = reader.GetInt32(0);
                string userName = reader.GetString(1);
                string productName = reader.GetString(2);
                Actions actionId = (Actions)reader.GetInt32(3);
                DateTime executionDate = reader.GetDateTime(4);

                bitacoraProducts.Add(new BitacoraProducts
                {
                    BitacoraLogId = bitacoraId,
                    UserName = userName,
                    ProductDescription = productName,
                    ActionID = actionId,
                    ExecutionDate = executionDate,
                });
            }

            return bitacoraProducts;
        }

        public async Task InsertAsync(int userId, int productId, Actions actionId)
        {
            const string query = @"
                INSERT INTO BitacoraProducts 
                    (userId, actionId, productId, executionDate) 
                VALUES 
                    (@userId, @actionId, @productId, GETDATE())
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@actionId", (int)actionId);
            command.Parameters.AddWithValue("@productId", productId);

            await command.ExecuteNonQueryAsync();
        }
    }
}