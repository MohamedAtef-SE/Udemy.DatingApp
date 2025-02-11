using API.DTOs.members;
using API.DTOs.messages;
using API.Entities;
using API.Helpers;
using API.Interfaces.Repositories;
using API.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class MessageRepository(DataContext _dbContext, IMapper _mapper) : IMessageRepository
    {
        public async Task<Message?> GetMessageAsync(int id)
        {
            return await _dbContext.Messages.FindAsync(id);
        }

        public Task<PagedList<MessageDTO>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var query = _dbContext.Messages
                                  .OrderByDescending(m => m.MessageSent)
                                  .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(m => m.Recipient.UserName == messageParams.Username
                                       && m.RecipientDeleted == false),

                "Outbox" => query.Where(m => m.Sender.UserName == messageParams.Username
                                       && m.SenderDeleted == false),
                _ => query.Where(m => m.Recipient.UserName == messageParams.Username 
                && m.DateRead == null && m.RecipientDeleted == false)
            };

            var messages = query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider);

            return PagedList<MessageDTO>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string otherUsername)
        {
            var messages = await _dbContext.Messages
                         .Include(m => m.Sender).ThenInclude(s => s.Photos)
                         .Include(m => m.Recipient).ThenInclude(r => r.Photos)
                         .Where(
                          m => m.SenderUserName == currentUsername
                               && m.RecipientUserName == otherUsername
                               && m.SenderDeleted == false
                               ||
                               m.RecipientUserName == currentUsername
                               && m.SenderUserName == otherUsername
                               && m.RecipientDeleted == false
                          )
                         .OrderBy(m => m.MessageSent)
                       //.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUserName == currentUsername);
                                        
            foreach (var message in unreadMessages)
            {
                message.DateRead = DateTime.UtcNow;
            }

            var isUpdated = await SaveAllAsync();

            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }

        public void AddMessage(Message message)
        {
            _dbContext.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _dbContext.Remove(message);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void AddGroup(Group group)
        {
            _dbContext.Groups.Add(group);
        }

        public void RemoveConnection(Connection connection)
        {
            _dbContext.Connections.Remove(connection);
        }

        public async Task<Connection?> GetConnectionAsync(string connectionId)
        {
            return await _dbContext.Connections.FindAsync(connectionId);
        }

        public async Task<Group?> GetMessageGroupAsync(string groupName)
        {
            return await _dbContext.Groups
                                   .Include(g => g.Connections)
                                   .FirstOrDefaultAsync(g => g.Name == groupName);
        }

        public async Task<Group?> GetGroupForConneciton(string connectionId)
        {
            return await _dbContext.Groups
                                   .Include(g => g.Connections)
                                   .Where(g => g.Connections.Any(c => c.ConnectionId == connectionId))
                                   .FirstOrDefaultAsync();
        }
    }
}
