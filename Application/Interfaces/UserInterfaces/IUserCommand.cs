using Application.Models.UserModels;

namespace Application.Interfaces.UserInterfaces
{
    public interface IUserCommand
    {
        Task<UserResponseDTO> InsertUser(UserRequestDTO user);
        Task<UserResponseDTO> UpdateUser(UserRequestDTO user);
        Task<UserResponseDTO> DeleteUser(UserRequestDTO user);
    }
}
