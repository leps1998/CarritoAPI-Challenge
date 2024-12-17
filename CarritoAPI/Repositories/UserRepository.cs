using CarritoAPI.Data;
using CarritoAPI.Domain;
using CarritoAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarritoAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CartDbContext _context;

        public UserRepository(CartDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetByDniAsync(string dni)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Dni == dni);
        }
    }
}
