using CarritoAPI.Domain;

namespace CarritoAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByDniAsync(string dni);
    }
}
