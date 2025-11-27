using Application.Models.Request;
using Application.Models.Response;
using MediatR;

namespace Application.Features.User.Command
{
    public record RegisterCommand(RegisterRequest request) : IRequest<GenericResponse>;
}
