using CQRS.Application.Helpers;
using CQRS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CQRS.Application._Commands.Admin.EditRoles
{
    internal class EditRolesCommandHandler(UserManager<AppUser> _userManager) : IRequestHandler<EditRolesCommand, Result<IEnumerable<string>>>
    {
        public async Task<Result<IEnumerable<string>>> Handle(EditRolesCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Roles)) 
                return Result<IEnumerable<string>>.Failure("you must select at least one role");

            var selectedRoles = request.Roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null) return Result<IEnumerable<string>>.Failure("User not found");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return Result<IEnumerable<string>>.Failure("failed to add roles to this user");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return Result<IEnumerable<string>>.Failure("failed to remove roles from this user");

            var currentUserRoles = await _userManager.GetRolesAsync(user);

            if (currentUserRoles is null) return Result<IEnumerable<string>>.Failure("failed to get user roles");

            return Result<IEnumerable<string>>.Success(currentUserRoles);

        }
    }
}
