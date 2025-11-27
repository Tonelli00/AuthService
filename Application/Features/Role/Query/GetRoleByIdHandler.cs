using Application.Interfaces.Query;
using Application.Models.Response;
using MediatR;

namespace Application.Features.Role.Query
{
    public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleResponse>
    {
        private readonly IRoleQuery _Query;

        public GetRoleByIdHandler(IRoleQuery query)
        {
            _Query = query;
        }

        public async Task<RoleResponse> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.roleId <= 0)
            {
                throw new ArgumentException($"El rol con el ID {request.roleId} es inválido");
            }

            var role = await _Query.GetByIdAsync(request.roleId, cancellationToken);

            if (role is null)
            {
                throw new KeyNotFoundException($"No se encontró un rol con el ID {request.roleId}");
            }

            return new RoleResponse
            {
                Id = role.RoleId,
                Name = role.Name
            };
        }
    }
}
