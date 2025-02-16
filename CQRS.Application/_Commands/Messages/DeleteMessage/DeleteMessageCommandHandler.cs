using CQRS.Application.Helpers;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Commands.Messages.DeleteMessage
{
    internal class DeleteMessageCommandHandler(IUnitOfWork _unitOfWork,IHttpContextAccessor _httpContextAccessor) : IRequestHandler<DeleteMessageCommand,Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User.GetUserName();
            var message = await _unitOfWork.MessageRepository.GetMessageAsync(request.MessageId);
            if (message is null) return Result<bool>.Failure("failed to find this message");

            if (message.SenderUserName != username && message.RecipientUserName != username)
                return Result<bool>.Failure("forbid");

            if (message.SenderUserName == username) message.SenderDeleted = true;
            if (message.RecipientUserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _unitOfWork.MessageRepository.DeleteMessage(message);
            }

            return Result<bool>.Success(await _unitOfWork.CompleteAsync());


        }
    }
}
