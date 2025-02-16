using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Commands.Users.UpdateUserProfilePicture
{
    public record UpdateUserProfilePictureCommand(int PhotoId):IRequest<Result<bool>>;
   
}
