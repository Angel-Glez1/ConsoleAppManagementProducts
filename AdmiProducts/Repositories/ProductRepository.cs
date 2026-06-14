using AdmiProducts.Models;
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
            this._connectionString = connectionString;
        }



        public Task Create(Product product)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> FindAll()
        {
            // 1. Crear objeto para la conxion.
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open(); ;


            // 2. Preparar el la query
            string query = @"SELECT productId, description, Quantity, estatusId FROM Products WHERE estatusId = 1";

       
            // 3. Ejecutamos la query y obtenemos resultados
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader result = command.ExecuteReader();

            
            // 4. Productos que vamos a regresar
            List<Product> products = new List<Product>();


            // 5. Leer resultaod de la query, obtener filas y agregar a la lista de productos un nuevo producto.
            while (result.Read())
            {
                int productId = result.GetInt32(0);
                string description = result.GetString(1);
                int quantity = result.GetInt32(2);
                int estatusId = result.GetInt32(3);

                products.Add(new Product(productId, description, quantity, estatusId));
            }


            // Liberamos conexión
            connection.Close();

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
    }
}
