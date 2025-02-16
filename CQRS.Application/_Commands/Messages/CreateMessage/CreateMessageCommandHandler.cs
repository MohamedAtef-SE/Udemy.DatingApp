using AutoMapper;
using CQRS.Application.DTOs.messages;
using CQRS.Application.Helpers;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Commands.Messages.CreateMessage
{
    public class CreateMessageCommandHandler(IHttpContextAccessor _httpContextAccessor, IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<CreateMessageCommand, Result<MessageDTO>>
    {
        public async Task<Result<MessageDTO>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User.GetUserName();
            if (username == request.MessageDTO.RecipientUserName.ToLower())
                return Result<MessageDTO>.Failure("It's a Good to talk with yourself, But not on our Application 😏");

            if (username is null) return Result<MessageDTO>.Failure("username not found");

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.MessageDTO.RecipientUserName);
            if (sender is null || recipient is null || sender.UserName is null || recipient.UserName is null)
                return Result<MessageDTO>.Failure("something wrong, sender or recipient is missing!!");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = request.MessageDTO.Content
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.CompleteAsync())
                return Result<MessageDTO>.Success(_mapper.Map<MessageDTO>(message));

            return Result<MessageDTO>.Failure("Something went wrong during returning the message");
        }
    }
}
