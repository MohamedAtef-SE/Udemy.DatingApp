using CQRS.Application.DTOs.messages;
using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Commands.Messages.CreateMessage
{
    public record CreateMessageCommand(MessageDTO MessageDTO) : IRequest<Result<MessageDTO>>;
}
