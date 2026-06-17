using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace AdmiProducts.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User?> GetUserByIdentifierAsync(string identifier)
        {
            const string query = @"
                SELECT
                     userId
                    ,name
                    ,estatusId
                FROM Users
                WHERE identifier = @identifier
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@identifier", identifier);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                int userId = reader.GetInt32(0);
                string name = reader.GetString(1);
                Estatus estatusId = (Estatus)reader.GetInt32(2);

                return new User(userId, identifier, name, estatusId);
            }

            return null;
        }
    }
}