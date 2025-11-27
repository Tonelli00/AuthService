using Application.Models.Response;
using MediatR;

namespace Application.Features.Role.Query
{
    public record GetAllRolesQuery() : IRequest<List<RoleResponse>>;
}
