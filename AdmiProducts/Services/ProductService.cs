using AdmiProducts.Exceptions;
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


        public ProductService(IProductRepository productrepository)
        {
            _productrepository = productrepository;
        }

        public async Task<List<Product>> FindAllAsync()
        {
            return await _productrepository.FindAll();
        }



        public async Task<int> CreateAsync(string description, int quantity)
        {

            // Validar que la description no se un string vacio
            if (string.IsNullOrEmpty(description))
                throw new BusinessException("El nombre del producto es obligatorio.");



            // Validar que el stock no sea negativo o 0.
            if (quantity <= 0)
                throw new BusinessException("El stock no puede ser 0 o un número negativo.");


            var result = await _productrepository.Create(description, quantity);

            // Validar si el producto se inserto en DB.
            if (result == 0)
                throw new BusinessException("No se logro realizar la trasaccion del SQL");

            return result;
        }


        public async Task<int> UpdateAsync(Product product)
        {
            // Validar que la description no se un string vacio
            if (string.IsNullOrEmpty(product.Description))
                throw new BusinessException("El nombre del producto es obligatorio.");



            // Validar que el stock no sea negativo o 0.
            if (product.Quantity <= 0)
                throw new BusinessException("El stock no puede ser 0 o un número negativo.");


            var result = await _productrepository.Update(product);

            // Validar si el producto se inserto en DB.
            if (result == 0)
                throw new BusinessException("No se logro realizar la trasaccion del SQL");

            return result;
        }



        public async Task DeleteAsync(int productId)
        {

            // Validar si el id solicitado existe
            var product = await _productrepository.FindById(productId);

            if (product is null)
                throw new BusinessException($"El producto con el id: {productId} no existe.");


            int result = await _productrepository.Delete(productId);

            if (result == 0)
                throw new BusinessException("No se logro realizar la trasaccion del SQL");
        }
    }

}
