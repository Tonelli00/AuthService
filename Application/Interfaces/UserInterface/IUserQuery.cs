using Application.Models.AuthModels.Login;
using Application.Models.UserModels;
using Domain.Entities;

namespace Application.Interfaces.UserInterface
{
    public interface IUserQuery
    {
        Task<bool> IsEmailUnique(string email);
        Task<User> GetById(Guid userId);
        Task<User> GetByEmail(string email);
        Task<List<User>> GetAllUsers();
    }
}
