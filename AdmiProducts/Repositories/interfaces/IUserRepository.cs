using AdmiProducts.Models;

namespace AdmiProducts.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdentifierAsync(string identifier);
    }
}