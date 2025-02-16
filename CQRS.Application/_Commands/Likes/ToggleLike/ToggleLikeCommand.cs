using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Commands.Likes.ToggleLike
{
    public record ToggleLikeCommand(int TargetUserId) : IRequest<Result<bool>>;
}
