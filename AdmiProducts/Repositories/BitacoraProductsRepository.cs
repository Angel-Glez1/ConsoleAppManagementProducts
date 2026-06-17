using AdmiProducts.Models;
using AdmiProducts.Models.Enums;
using AdmiProducts.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Repositories
{
    public class BitacoraProductsRepository : IBitacoraProductsRepository
    {

        private readonly string _connectionString;

        public BitacoraProductsRepository(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public async Task<List<BitacoraProducts>> Get(int userId)
        {
            string query = @"
                   SELECT 
                       bitacoraId
                      ,Users.name
                      ,Products.description
                      ,Actions.actionId
                      ,executionDate
                  FROM [ProyectoIngSoftware].[dbo].[BitacoraProducts]
                  JOIN Users ON Users.userId = BitacoraProducts.userId
                  JOIN Products ON Products.productId = BitacoraProducts.bitacoraId
                  JOIN Actions ON Actions.actionId = BitacoraProducts.actionId
                  WHERE Users.userId = @userId
                  ORDER BY 1 DESC;
            ";



            // Crear objeto de conexción,
            using var connection = new SqlConnection(_connectionString);

            //Abrir la conexción
            await connection.OpenAsync();

            // Crear objeto que permite ejecutar la query
            using var command = new SqlCommand(query, connection);

            //Parameterized Queries
            command.Parameters.AddWithValue("@userId", userId);

            //Ejecutar query
            using var reader = await command.ExecuteReaderAsync();

            List<BitacoraProducts> bitacoraProducts = new();

            //Leer el resultado y crear un objeto.
            while (await reader.ReadAsync())
            {
                int bitacoraId = reader.GetInt32(0);
                string userName = reader.GetString(1);
                string productName = reader.GetString(2);
                Actions actiondId = (Actions)reader.GetInt32(3);
                DateTime executionDate = reader.GetDateTime(4);


                bitacoraProducts.Add(new BitacoraProducts()
                {
                    BitacoraLogId = bitacoraId,
                    UserName = userName,
                    ProductDescription = productName,
                    ActionID = actiondId,
                    ExecutionDate = executionDate,
                });
            }


            return bitacoraProducts;
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


            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected;
        }
    }
}
