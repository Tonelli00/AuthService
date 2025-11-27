using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Query
{
    public record GetAllUsersQuery() : IRequest<List<UserResponse>>;
}
