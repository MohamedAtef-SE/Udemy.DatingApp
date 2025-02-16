using CQRS.Application.Helpers;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.DTOs.auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Application._Queries.Admin.UsersWithRolsQuery
{
    internal class GetUserWithRolesQueryHandler(UserManager<AppUser> _userManager) : IRequestHandler<GetUserWithRolesQuery, Result<IEnumerable<UserWithRoleDTO>>>
    {
        public async Task<Result<IEnumerable<UserWithRoleDTO>>> Handle(GetUserWithRolesQuery request, CancellationToken cancellationToken)
        {
            var userswithRoles = await _userManager.Users
                              .OrderBy(user => user.UserName)
                              .Select(user => new UserWithRoleDTO
                              {
                                  Id = user.Id,
                                  Username = user.UserName,
                                  Roles = user.UserRoles.Select(userRole => userRole.Role.Name).ToList()
                              })
                              .ToListAsync();

            if (userswithRoles is not null) return Result<IEnumerable<UserWithRoleDTO>>.Success(userswithRoles);

            throw new Exception("somehing went wrong during fetching users with roles from database.");
        }
    }
}
