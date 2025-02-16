using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Queries.Users.GetUserById
{
    public record GetUserByIdQuery(int Id):IRequest<Result<MemberDTO>>;
   
}
