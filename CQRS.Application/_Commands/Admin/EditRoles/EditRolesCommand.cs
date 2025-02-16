using CQRS.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Application._Commands.Admin.EditRoles
{
    public record EditRolesCommand(string Username, [FromQuery] string Roles) : IRequest<Result<IEnumerable<string>>>;

}
