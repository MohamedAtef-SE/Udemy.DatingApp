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

        public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _dbContext.Messages
                         .Include(m => m.Sender).ThenInclude(s => s.Photos)
                         .Include(m => m.Recipient).ThenInclude(s => s.Photos)
                         .Where(
                          m => m.SenderUserName == currentUsername 
                               && m.RecipientUserName == recipientUsername
                               && m.SenderDeleted == false
                               ||
                               m.RecipientUserName == currentUsername
                               && m.SenderUserName == recipientUsername 
                               && m.RecipientDeleted == false
                          )
                         .OrderBy(m => m.MessageSent)
                         .ToListAsync();
            var unreadMessages = messages.Where(m => m.DateRead is null
            && m.RecipientUserName == currentUsername).ToList();

            if (unreadMessages.Count != 0)
            {
                unreadMessages.ForEach(m => m.DateRead = DateTime.UtcNow);
                await _dbContext.SaveChangesAsync();
            }

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
    }
}
