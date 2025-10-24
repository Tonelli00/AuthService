using Application.Interfaces.UserInterfaces;
using Application.Models.UserModels;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Commands.UserCommand
{
    public class UserCommand : IUserCommand
    {
        private readonly AppDbContext _context;
        public UserCommand(AppDbContext context)
        {
            _context = context;
        }

        public Task<UserResponseDTO> DeleteUser(UserRequestDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponseDTO> InsertUser(UserRequestDTO user)
        {
            User u = new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId
            };
            await _context.Users.AddAsync(u);
            await _context.SaveChangesAsync();
            UserResponseDTO userResponse = new UserResponseDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password,
                Phone = u.Phone,
                RoleId = u.RoleId
            };
            return userResponse;
        }

        public Task<UserResponseDTO> UpdateUser(UserRequestDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
