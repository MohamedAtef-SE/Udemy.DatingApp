using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Commands.Messages.DeleteMessage
{
    public record DeleteMessageCommand(int MessageId): IRequest<Result<bool>>;
    
}
