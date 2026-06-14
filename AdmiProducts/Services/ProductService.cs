using AdmiProducts.Models;
using AdmiProducts.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Services
{
    public class ProductService
    {

        private readonly IProductRepository _repository;


        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Product>> getAllProducts()
        {
            return await _repository.FindAll();
        }


    }
}
