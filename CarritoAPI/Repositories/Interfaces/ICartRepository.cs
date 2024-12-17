using CarritoAPI.Domain;
using CarritoAPI.DTOs;

namespace CarritoAPI.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByIdAsync(int id);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(int id);
        Task<IEnumerable<Cart>> GetCartsByUserIdAsync(int userId);
        Task<IEnumerable<Product>> GetTopProductsByUserAsync(int userId, int top);
    }
}
