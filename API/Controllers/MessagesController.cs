using API.Controllers._common;
using API.DTOs.messages;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessagesController(IUnitOfWork _unitOfWork, IMapper _mapper) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO messageDTO)
        {
            var username = User.GetUserName();
            if (username == messageDTO.RecipientUserName.ToLower())
                return BadRequest("It's a Good to talk with yourself, But not on our Application 😏");

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(messageDTO.RecipientUserName);
            if (sender is null || recipient is null || sender.UserName is null || recipient.UserName is null)
                return BadRequest("Cannot send message at this time.");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = messageDTO.Content
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.CompleteAsync())
                return Ok(_mapper.Map<MessageDTO>(message));

            return BadRequest("Failed to save message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUserName();
            var messages = await _unitOfWork.MessageRepository.GetMessagesForUserAsync(messageParams);
            Response.AddPaginationHeader<MessageDTO>(messages);
            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUserName();

            return Ok(await _unitOfWork.MessageRepository.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUserName();
            var message = await _unitOfWork.MessageRepository.GetMessageAsync(id);
            if (message == null) return BadRequest("Cannot Delete this message");

            if (message.SenderUserName != username && message.RecipientUserName != username)
                return Forbid();

            if(message.SenderUserName == username) message.SenderDeleted = true;
            if(message.RecipientUserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if (await _unitOfWork.CompleteAsync()) 
                return Ok();

            return BadRequest("Problem deleting the message");    
        }

    }
}
