using CarritoAPI.Domain;
using CarritoAPI.DTOs;

namespace CarritoAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<int> CreateCartAsync(string dni);
        Task DeleteCartAsync(int cartId);
        Task AddProductAsync(int cartId, int productId, int quantity);
        Task DeleteProductAsync(int cartId, int productId);
        Task<CartStatusDTO> GetCartStatus(int cartId);

        Task<IEnumerable<Product>> GetExpensiveProductPerUserAsync(string dni);
    }
}
