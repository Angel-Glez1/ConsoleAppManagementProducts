using AdmiProducts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdentifier(string identifier);
    }
}
