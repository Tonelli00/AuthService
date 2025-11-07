using Application.Interfaces.HelperInterface;
using Application.Interfaces.UserInterface;
using Application.Models.AuthModels.Login;
using Application.Models.AuthModels.Register;
using Application.Models.UserModels;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Application.UseCase.UserUseCase
{
    public class UserService : IUserService
    {
        private readonly IUserCommand _userCommand;
        private readonly IUserQuery _userQuery;
        private readonly IConfiguration _configuration;
        private readonly IHashingService _hash;
        public UserService(IUserCommand userCommand, IUserQuery userQuery, IConfiguration configuration,
            IHashingService hash)
        {
            _userCommand = userCommand;
            _userQuery = userQuery;
            _configuration = configuration;
            _hash = hash;
        }
        public async Task<UserResponseDTO> DeleteUser(Guid userId)
        {
            await _userCommand.DeleteUser(userId);
            User user = await _userQuery.GetById(userId);
            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            };
        }

        public async Task<List<UserResponseDTO>> GetAllUsers()
        {
            List<User> users = await _userQuery.GetAllUsers();
            return users.Select(user => new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            }).ToList();
        }

        public async Task<UserResponseDTO> GetUser(Guid userId)
        {
            User user = await _userQuery.GetById(userId);
            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            };
        }

        public async Task<string> LoginUser(LoginDTO request)
        {
            var Vmails = new string[] { "@gmail.com", "@outlook.com", "@hotmail.com", "@yahoo.com" };

            if (string.IsNullOrWhiteSpace(request.Email) || !Vmails.Any(vm=>request.Email.Contains(vm)))
            {
                throw new ArgumentException("Ingrese un mail válido");
            }


            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Ingrese una contraseña válida");
            }

            var user = await _userQuery.GetByEmail(request.Email);

            if (user == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario.");
            }
            
            var hashedInput = _hash.encryptSHA256(request.Password);

            if (user.Password != hashedInput)
                throw new ArgumentException("Contraseña Incorrecta.");

            var userDto=new LoginResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                RoleName = user.Role.Name,
                Password = user.Password
            };



            string token = GenerateJwtToken(new LoginResponseDTO
            {
                Id = userDto.Id,
                Email = userDto.Email,
                RoleName = userDto.RoleName
            });

            return token;

        }

        public async Task<RegisterResponseDTO> RegisterUser(RegisterRequestDTO request)
        {
             var carac = new string[] {"@","_","-","$","#","&","/" };
            var Vmails = new string[] { "@gmail.com", "@outlook.com", "@hotmail.com", "@yahoo.com" };
           
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Phone))
            {
                throw new ArgumentException("Complete los campos solicitados...");
            }

            if (!Vmails.Any(Vm=>request.Email.Contains(Vm))) 
            {
                throw new ArgumentException("Ingrese un Email válido");
            }
            if (request.Password.Length <= 8) 
            {
                throw new ArgumentException("Ingrese una contraseña segura (Que contenga más de 8 caracteres)");
            }
            if (!carac.Any(c=>request.Password.Contains(c)))
            {
                throw new ArgumentException("La contraseña debe contener caracteres especiales. Ejemplo: @, _, -, $, #, &, /)");
            }
            if (request.Phone.Length < 10) 
            {
                throw new ArgumentException("Ingrese un numero de telefono válido");
            }

            User user = await _userCommand.InsertUser(new User
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                Password = _hash.encryptSHA256(request.Password),
                Phone = request.Phone,
                RoleId = 1
            });
            return new RegisterResponseDTO
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email
            };
        }


        public async Task<UserResponseDTO> UpdateUser(User userDto)
        {
            User user = await _userCommand.UpdateUser(new User
            {
                Id = userDto.Id,
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = _hash.encryptSHA256(userDto.Password),
                Phone = userDto.Phone,
                RoleId = userDto.RoleId
            });
            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId
            };
        }

        private string GenerateJwtToken(LoginResponseDTO user)
        {
            var userClaims = new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.RoleName),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
