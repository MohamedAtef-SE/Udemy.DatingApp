using API.Controllers._common;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController(UserManager<AppUser> _userManager):BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var userswithRoles = await _userManager.Users
                              .OrderBy(user => user.UserName)
                              .Select(user => new
                              {
                                  user.Id,
                                  username = user.UserName,
                                  Roles = user.UserRoles.Select(userRole => userRole.Role.Name).ToList()
                              })
                              .ToListAsync();
            return Ok(userswithRoles);
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit/roles/{username}")]
        public async Task<ActionResult> EditRoles(string username,[FromQuery]string roles)
        {
            if(string.IsNullOrEmpty(roles)) return BadRequest("you must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user is null) return BadRequest("User not found");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles));
            if (!result.Succeeded) return BadRequest("failed to add roles to this user");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded) return BadRequest("failed to remove roles from this user");
            return Ok(await _userManager.GetRolesAsync(user));
        }
    }
}
