using CQRS.Application.Helpers;
using CQRS.Infrastructure.DTOs.auth;
using MediatR;

namespace CQRS.Application._Queries.Admin.UsersWithRolsQuery
{
    public record GetUserWithRolesQuery : IRequest<Result<IEnumerable<UserWithRoleDTO>>>;

}
