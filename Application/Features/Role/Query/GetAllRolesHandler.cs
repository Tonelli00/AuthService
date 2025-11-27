using Application.Interfaces.Query;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Role.Query
{
    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, List<RoleResponse>>
    {
        private readonly IRoleQuery _query;

        public GetAllRolesHandler(IRoleQuery query)
        {
            _query = query;
        }

        public async Task<List<RoleResponse>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _query.GetAllAsync(cancellationToken);

            return roles.Select(role => new RoleResponse
            {
                Id = role.RoleId,
                Name = role.Name
            }).ToList();
        }
    }
}
