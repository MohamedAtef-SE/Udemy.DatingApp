using CQRS.Application.DTOs.messages;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;

namespace CQRS.Infrastructure.Interfaces.Repositories
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
