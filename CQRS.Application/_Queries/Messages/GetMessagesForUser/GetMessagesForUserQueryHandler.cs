using CQRS.Application.DTOs.messages;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using CQRS.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Queries.Messages.GetMessagesForUser
{
    public class GetMessagesForUserQueryHandler(IUnitOfWork _unitOfWork,IHttpContextAccessor _httpContextAccessor) : IRequestHandler<GetMessagesForUserQuery, Result<PagedList<MessageDTO>>>
    {
        public async Task<Result<PagedList<MessageDTO>>> Handle(GetMessagesForUserQuery request, CancellationToken cancellationToken)
        {
            request.MessageParams.Username = _httpContextAccessor.HttpContext?.User.GetUserName();
            var messages = await _unitOfWork.MessageRepository.GetMessagesForUserAsync(request.MessageParams);
            if(messages is not null) return Result<PagedList<MessageDTO>>.Success(messages);

            throw new Exception("somehing went wrong during fetching messages from database.");
        }
    }
}
