namespace API.DTOs.messages
{
    public class CreateMessageDTO
    {
        public required string RecipientUserName { get; set; }
        public required string Content { get; set; }
    }
}
