using AdmiProducts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> FindAll();
        
        Task<Product?> FindById(int id);
        Task<int> Create(string description, int quantity);
        Task<int> Update(Product product);
        Task<int> Delete(int id);
    }
}
