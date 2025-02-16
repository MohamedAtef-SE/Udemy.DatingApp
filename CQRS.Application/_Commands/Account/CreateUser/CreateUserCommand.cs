using CQRS.Application.DTOs.auth;
using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Commands.Account.CreateUser
{
    public record CreateUserCommand(RegisterDTO RegisterDTO) : IRequest<Result<UserDTO>>;

}
