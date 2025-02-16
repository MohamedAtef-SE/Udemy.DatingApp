using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using CQRS.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CQRS.Application._Queries.Users.GetUsers
{
    public class GetUsersQueryHandler(IHttpContextAccessor _httpContextAccessor,IUnitOfWork _unitOfWork) : IRequestHandler<GetUsersQuery, Result<PagedList<MemberDTO>>>
    {
        public async Task<Result<PagedList<MemberDTO>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User.GetUserName();
            if (username is null) return Result<PagedList<MemberDTO>>.Failure("username is not found");

            request.UserQueryParams.CurrentUsername = username;
            var users = await _unitOfWork.UserRepository.GetMembersAsync(request.UserQueryParams);
            if(users is not null) return Result<PagedList<MemberDTO>>.Success(users);

            throw new Exception("somehing went wrong during fetching users from database.");
        }
    }
}
