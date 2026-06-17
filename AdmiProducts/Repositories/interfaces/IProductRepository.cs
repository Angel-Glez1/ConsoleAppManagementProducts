using AdmiProducts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> FindAllAsync();
        Task<Product?> FindByIdAsync(int productId);
        Task<int> CreateAsync(string description, int quantity);
        Task<int> UpdateAsync(Product product);
        Task<int> DeleteAsync(int productId);
    }
}