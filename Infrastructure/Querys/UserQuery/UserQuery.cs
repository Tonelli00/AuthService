using Application.Interfaces.Query;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Querys.UserQuery
{
    public class UserQuery : IUserQuery
    {
        private readonly AppDbContext _context;
        public UserQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistUser(Guid id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            User user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> GetById(Guid userId)
        {
            User user = await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<bool> IsEmailUnique(string email)
        {
            return !await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
