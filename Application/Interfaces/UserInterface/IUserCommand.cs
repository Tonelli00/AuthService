using Domain.Entities;

namespace Application.Interfaces.UserInterface
{
    public interface IUserCommand
    {
        Task<User> InsertUser(User user);
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(Guid Id);
    }
}
