using AdmiProducts.Models;
using AdmiProducts.Repositories;
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

        public IEnumerable<Product> getAllProducts()
        {
            return _repository.FindAll();
        }


    }
}
