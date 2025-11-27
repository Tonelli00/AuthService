using Application.Models.Response;
using MediatR;

namespace Application.Features.Role.Query
{
    public record GetRoleByIdQuery(int roleId) : IRequest<RoleResponse>;
}
