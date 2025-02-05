using API.Entities;

namespace API.DTOs.messages
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public required string SenderUserName { get; set; }
        public required string SenderPhotoURL { get; set; }
        public int RecipientId { get; set; }
        public required string RecipientUserName { get; set; }
        public required string RecipientPhotoURL { get; set; }
        public required string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } 
    }
}
