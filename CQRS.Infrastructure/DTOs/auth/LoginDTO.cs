using System.ComponentModel.DataAnnotations;

namespace CQRS.Application.DTOs.auth
{
    public class LoginDTO
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
