using AdmiProducts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> FindAll();
        Task<Product?> FindById(int id);
        Task Create(Product product);
        Task Update(Product product);
        Task Delete(int id);
    }
}
