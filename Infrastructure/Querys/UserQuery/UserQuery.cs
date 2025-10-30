using Application.Interfaces.UserInterface;
using Application.Models.AuthModels.Login;
using Application.Models.UserModels;
using Application.UseCase.HashUseCase;
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
        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
            return users;
        }

        public async Task<LoginResponseDTO> GetByEmail(string email)
        {
            User user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
            return new LoginResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                RoleName = user.Role.Name,
                Password = user.Password
            };
        }

        public async Task<User> GetById(Guid userId)
        {
            User user = await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<bool> IsEmailUnique(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
