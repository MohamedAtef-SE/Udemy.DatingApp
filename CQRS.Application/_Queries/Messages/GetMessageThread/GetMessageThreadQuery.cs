using CQRS.Application.DTOs.messages;
using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Queries.Messages.GetMessageThread
{
    public record GetMessageThreadQuery(string Username): IRequest<Result<IEnumerable<MessageDTO>>>;
}
