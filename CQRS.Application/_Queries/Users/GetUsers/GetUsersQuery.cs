using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;
using MediatR;

namespace CQRS.Application._Queries.Users.GetUsers
{
    public record GetUsersQuery(UserQueryParams UserQueryParams):IRequest<Result<PagedList<MemberDTO>>>;
    
}
