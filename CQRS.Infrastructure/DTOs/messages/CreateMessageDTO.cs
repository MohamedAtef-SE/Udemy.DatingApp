namespace CQRS.Application.DTOs.messages
{
    public class CreateMessageDTO
    {
        public required string RecipientUserName { get; set; }
        public required string Content { get; set; }
    }
}
