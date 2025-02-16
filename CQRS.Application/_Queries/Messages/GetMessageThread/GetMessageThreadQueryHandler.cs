using CQRS.Application.DTOs.messages;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Queries.Messages.GetMessageThread
{
    internal class GetMessageThreadQueryHandler(IHttpContextAccessor _httpContextAccessor,IUnitOfWork _unitOfWork) : IRequestHandler<GetMessageThreadQuery, Result<IEnumerable<MessageDTO>>>
    {
        public async Task<Result<IEnumerable<MessageDTO>>> Handle(GetMessageThreadQuery request, CancellationToken cancellationToken)
        {
            var currentUsername = _httpContextAccessor.HttpContext?.User.GetUserName();
            if (currentUsername is null) return Result<IEnumerable<MessageDTO>>.Failure("username not found");
            var messages = await _unitOfWork.MessageRepository.GetMessageThread(currentUsername, request.Username);

            if(messages is not null) return Result<IEnumerable<MessageDTO>>.Success(messages);

            throw new Exception("somehing went wrong during fetching messages from database.");
        }
    }
}
