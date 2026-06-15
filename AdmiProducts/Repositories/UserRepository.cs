using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionstring;
        public UserRepository(string connectionstring)
        {
            _connectionstring = connectionstring;
        }


        public async Task<User?> GetUserByIdentifier(string identifier)
        {
            User? user = null;

            // 1. Crear objeto con el pollConection y abrir la conexcion
            using var connection = new SqlConnection(_connectionstring);
            await connection.OpenAsync();

            // 2. Crea y ejecuta la query
            string query = @"
                 SELECT
	                 userId
	                ,name
	                ,estatusId
                FROM Users
                WHERE identifier = @identifier
            ";

            // 2. Prepara el sqlCommand y sustituye el placeholder
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@identifier", identifier);


            // 3. Ejecutamos query
            using var reader = await command.ExecuteReaderAsync();


            // 4. Valida si existe un usuario con ese identificador
            bool existsUser = reader.HasRows;
            if (!existsUser) {
                // En este punto la variable user es null, porque asi la inicializamos
                return user; 
            }


            // 5. Leé el resultado y asigna las propiedade al objeto de tipo User.
            while (await reader.ReadAsync())
            {
                int userId = reader.GetInt32(0);
                string name = reader.GetString(1);
                Estatus estatusId = (Estatus)reader.GetInt32(2);

                user = new User(userId, identifier, name, estatusId);
            }

            return user;

        }
    }
}
