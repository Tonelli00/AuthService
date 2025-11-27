using Application.Interfaces.UserInterface;
using Application.Models.Request;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commands.UserCommand
{
    public class UserCommand : IUserCommand
    {
        private readonly AppDbContext _context;
        public UserCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
                return false;

            user.Password = request.NewPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ChangeUserRole(Guid userId, int newRole)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            user.RoleId = newRole;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User> InsertUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
