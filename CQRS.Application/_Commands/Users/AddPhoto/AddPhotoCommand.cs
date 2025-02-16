using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Commands.Users.AddPhoto
{
    public record AddPhotoCommand(IFormFile File):IRequest<Result<PhotoDTO>>;
}
