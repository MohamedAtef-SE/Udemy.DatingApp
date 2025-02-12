using API.DTOs.messages;
using API.Entities;
using API.Helpers;
using API.Pagination;

namespace API.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message?> GetMessageAsync(int id);
        Task<PagedList<MessageDTO>> GetMessagesForUserAsync(MessageParams messageParams);
        Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername);
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection?> GetConnectionAsync(string connectionId);
        Task<Group?> GetMessageGroupAsync(string groupName);
        Task<Group?> GetGroupForConneciton(string connectionId);

    }
}
