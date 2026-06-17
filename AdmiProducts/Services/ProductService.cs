using AdmiProducts.Exceptions;
using AdmiProducts.Models;
using AdmiProducts.Repositories.Interfaces;

namespace AdmiProducts.Services
{
    public class ProductService
    {
        private const string ERROR_TRANSACCION_SQL = "No se logró realizar la transacción en la base de datos.";

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> FindAllAsync()
        {
            return await _productRepository.FindAllAsync();
        }

        public async Task<Product?> FindByIdAsync(int productId)
        {
            return await _productRepository.FindByIdAsync(productId);
        }

        public async Task<int> CreateAsync(string description, int quantity)
        {
            if (string.IsNullOrEmpty(description))
                throw new BusinessException("El nombre del producto es obligatorio.");

            if (quantity <= 0)
                throw new BusinessException("El stock no puede ser 0 o un número negativo.");

            var result = await _productRepository.CreateAsync(description, quantity);

            if (result == 0)
                throw new BusinessException(ERROR_TRANSACCION_SQL);

            return result;
        }

        public async Task<int> UpdateAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.Description))
                throw new BusinessException("El nombre del producto es obligatorio.");

            if (product.Quantity <= 0)
                throw new BusinessException("El stock no puede ser 0 o un número negativo.");

            var existente = await _productRepository.FindByIdAsync(product.ProductId);
            if (existente is null)
                throw new BusinessException($"El producto con el id: {product.ProductId} no existe.");

            var result = await _productRepository.UpdateAsync(product);

            if (result == 0)
                throw new BusinessException(ERROR_TRANSACCION_SQL);

            return result;
        }

        public async Task DeleteAsync(int productId)
        {
            var product = await _productRepository.FindByIdAsync(productId);

            if (product is null)
                throw new BusinessException($"El producto con el id: {productId} no existe.");

            int result = await _productRepository.DeleteAsync(productId);

            if (result == 0)
                throw new BusinessException(ERROR_TRANSACCION_SQL);
        }
    }
}