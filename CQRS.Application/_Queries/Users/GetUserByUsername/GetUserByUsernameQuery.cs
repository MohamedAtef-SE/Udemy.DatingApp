using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Queries.Users.GetUserByUsername
{
    public record GetUserByUsernameQuery(string Username) : IRequest<Result<MemberDTO>>;
   
}
