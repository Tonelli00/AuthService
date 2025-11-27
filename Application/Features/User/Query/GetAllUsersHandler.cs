using Application.Features.User.Query;
using Application.Interfaces.Query;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Handlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
    {
        private readonly IUserQuery _userQuery;

        public GetAllUsersHandler(IUserQuery userQuery)
        {
            _userQuery = userQuery;
        }

        public async Task<List<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userQuery.GetAll();

            return users.Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role.Name
            }).ToList();
        }
    }
}
