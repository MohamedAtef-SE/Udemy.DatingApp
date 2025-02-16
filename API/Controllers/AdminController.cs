using API.Controllers._common;
using CQRS.Application._Commands.Admin.EditRoles;
using CQRS.Application._Queries.Admin.UsersWithRolsQuery;
using CQRS.Infrastructure.DTOs.auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController(IMediator _mediator):BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult<IEnumerable<UserWithRoleDTO>>> GetUsersWithRoles()
        {
            var getUsersWithRoles = new GetUserWithRolesQuery();
            var result = await _mediator.Send(getUsersWithRoles);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit/roles/{username}")]
        public async Task<ActionResult<IEnumerable<string>>> EditRoles(string username, [FromQuery] string roles)
        {
            var editRolesCommand = new EditRolesCommand(username,roles);
            
            var result = await _mediator.Send(editRolesCommand);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
