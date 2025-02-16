using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Commands.Users.DeletePhoto
{
    public record DeletePhotoCommand(int PhotoId):IRequest<Result<bool>>;
    
}
