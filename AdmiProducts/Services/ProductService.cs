using AdmiProducts.Models;
using AdmiProducts.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Services
{
    public class ProductService
    {

        private readonly IProductRepository _productrepository;
        private readonly IBitacoraProducts _bitacorarepository;


        public ProductService(IProductRepository productrepository, IBitacoraProducts bitacorarepository)
        {
            _productrepository = productrepository;
            _bitacorarepository = bitacorarepository;
        }

        public async Task<List<Product>> getAllProducts()
        {
            return await _productrepository.FindAll();
        }




    }
}
