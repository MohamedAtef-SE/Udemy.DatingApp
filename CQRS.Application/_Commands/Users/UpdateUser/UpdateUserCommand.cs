using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Commands.Users.UpdateUser
{
    public record UpdateUserCommand(UpdateMemberDTO UpdateMemberDTO): IRequest<Result<bool>>;
   
}
