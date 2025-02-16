using CQRS.Application.DTOs.messages;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;
using MediatR;

namespace CQRS.Application._Queries.Messages.GetMessagesForUser
{
    public record GetMessagesForUserQuery(MessageParams MessageParams) : IRequest<Result<PagedList<MessageDTO>>>;
    
}
