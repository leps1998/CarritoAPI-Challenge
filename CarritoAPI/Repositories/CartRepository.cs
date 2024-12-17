using CarritoAPI.Data;
using CarritoAPI.Domain;
using CarritoAPI.DTOs;
using CarritoAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarritoAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _context;
        public CartRepository(CartDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartsByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetTopProductsByUserAsync(int userId, int top)
        {
            
            return await _context.Items
                .Where(item => item.Cart.UserId == userId) 
                .Select(item => item.Product)                     
                .Distinct()                                       
                .OrderByDescending(producto => producto.Price)   
                .Take(top)                                       
                .ToListAsync();
        }
    }
}
