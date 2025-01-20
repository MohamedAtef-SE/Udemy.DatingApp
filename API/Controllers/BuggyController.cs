using API.Data;
using API.DTOs.auth;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController(DataContext dataContext) : BaseApiController
    {
       
        [Authorize]
        [HttpGet("auth")] // {{BaseURL}}/api/buggy/auth
        public ActionResult GetUnauthorizedException()
        {

            return Ok("secret-text");
        }

        [HttpGet("not-found")] // {{BaseURL}}/api/buggy/not-found
        public ActionResult GetNotFoundException()
        {
            return NotFound(new { message = "this thing is not found!!" });
        }

        [HttpGet("bad-request")] // {{BaseURL}}/api/buggy/bad-request
        public ActionResult GetBadRequest()
        {
            return BadRequest(new { message = "this was not a good request!!" });
        }

        [HttpPost("validation")] // {{BaseURL}}/api/buggy/validation
        public ActionResult<RegisterDTO> GetValidationException(RegisterDTO user)
        {
            
            return Ok(user);
        }

        [HttpGet("server-error")] // {{BaseURL}}/api/buggy/server-error
        public ActionResult<AppUser> GetServerError()
        {
            var user = dataContext.Users.Find(-1) ?? throw new Exception("just a bad thing has happend");

            return Ok(user);
        }
    }
}
