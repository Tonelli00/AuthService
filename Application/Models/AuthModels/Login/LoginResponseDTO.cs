

namespace Application.Models.AuthModels.Login
{
    public class LoginResponseDTO
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public  string? UserLastName { get; set; }
        public string? UserPhone { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string? Password { get; set; }
    }
}
