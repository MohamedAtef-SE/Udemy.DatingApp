using API.Controllers._common;
using CQRS.Application._Commands.Account.CreateUser;
using CQRS.Application._Commands.Account.LoginUser;
using CQRS.Application.DTOs.auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController(IMediator _mediator) : BaseApiController
    {
        [HttpPost("register")] // {{BaseURL}}/api/account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var userCommand = new CreateUserCommand(registerDTO);
            var result = await _mediator.Send(userCommand);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var userCommand = new LoginUserCommand(loginDTO);
            var result = await _mediator.Send(userCommand);

            if (!result.IsSuccess) return BadRequest(new { message = result.ErrorMessage});
           
            return Ok(result.Value);
        }
    }


}
