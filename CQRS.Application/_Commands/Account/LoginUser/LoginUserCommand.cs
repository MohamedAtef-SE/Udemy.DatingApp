using CQRS.Application.DTOs.auth;
using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Commands.Account.LoginUser
{
    public record LoginUserCommand(LoginDTO LoginDTO) : IRequest<Result<UserDTO>>;
}
