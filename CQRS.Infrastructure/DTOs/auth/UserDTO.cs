namespace CQRS.Application.DTOs.auth
{
    public class UserDTO
    {
        public required string Username { get; set; }
        public required string Gender { get; set; }
        public string? PhotoURL { get; set; }
        public required string Token { get; set; }
        public required string KnownAs { get; set; }

    }
}
