using Application.Models.UserModels;

namespace Application.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> Login(UserLoginDTO login);
        Task<List<UserResponseDTO>> GetAllUsers();
        Task<UserResponseDTO> RegisterUser(UserRequestDTO user);
        Task<UserResponseDTO> UpdateUser(UserRequestDTO user);
        Task<UserResponseDTO> DeleteUser(UserRequestDTO user);
    }
}
