using API.Controllers._common;
using API.Extentions;
using CQRS.Application._Commands.Messages.CreateMessage;
using CQRS.Application._Commands.Messages.DeleteMessage;
using CQRS.Application._Queries.Messages.GetMessagesForUser;
using CQRS.Application._Queries.Messages.GetMessageThread;
using CQRS.Application.DTOs.messages;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessagesController(IMediator _mediator) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<MessageDTO?>> CreateMessage(MessageDTO messageDTO)
        {
            var messageCommand = new CreateMessageCommand(messageDTO);

            var result = await _mediator.Send(messageCommand);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDTO>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            var messagesForUserQuery = new GetMessagesForUserQuery(messageParams);
            var result = await _mediator.Send(messagesForUserQuery);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            
            Response.AddPaginationHeader<MessageDTO>(result.Value!);
            return Ok(result.Value);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username)
        {
            var messageThreadQuery = new GetMessageThreadQuery(username);

            var result = await _mediator.Send(messageThreadQuery);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var deleteMessageCommand = new DeleteMessageCommand(id);
            var result = await _mediator.Send(deleteMessageCommand);
            
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok();
        }

    }
}
