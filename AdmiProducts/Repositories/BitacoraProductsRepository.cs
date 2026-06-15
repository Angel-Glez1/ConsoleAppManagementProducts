using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Repositories
{
    public class BitacoraProductsRepository : IBitacoraProducts
    {

        private readonly string _connectionString;

        public BitacoraProductsRepository(string connectionstring)
        {
            _connectionString = connectionstring;
        }


        public async Task<int> Insert(int userId, int productId, Actions actionId)
        {
            string query = @"INSERT INTO BitacoraProducts 
                ( userId, actionId, productId, executionDate ) 
            VALUES 
                ( @userId, @actionId, @productId, GETDATE() ) 
            ";


            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();


            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@actionId", actionId);
            command.Parameters.AddWithValue("@productId", productId);


            int rowsAffected =  await command.ExecuteNonQueryAsync();
            return rowsAffected;
        }
    }
}
