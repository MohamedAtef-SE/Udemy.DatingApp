using API.DTOs.messages;
using API.Entities;
using API.Extentions;
using API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub(IMessageRepository messageRepository, IUserRepository _userRepository, IMapper _mapper, IHubContext<PresenceHub> presenceHub) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["user"];
            if (Context.User is null || string.IsNullOrEmpty(otherUser)) throw new HubException("Cannot join group");

            var groupName = GetGroupName(Context.User.GetUserName(), otherUser!);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Group? group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await messageRepository.GetMessageThread(Context.User.GetUserName(), otherUser!);
           
            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

       
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup",group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDTO messageDTO)
        {
            if (Context.User is null) throw new Exception("user not found");

            var username = Context.User.GetUserName();
            if (username == messageDTO.RecipientUserName.ToLower())
                throw new HubException("It's a Good to talk with yourself, But not on our Application 😏");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(messageDTO.RecipientUserName);
            if (sender is null || recipient is null || sender.UserName is null || recipient.UserName is null)
                throw new HubException("Cannot send message at this time.");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = messageDTO.Content
            };


            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await messageRepository.GetMessageGroupAsync(groupName);

            // if recipient user is already in the chatroom
            if (group is not null && group.Connections.Any(connection => connection.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connectionIds = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
                if (connectionIds is not null && connectionIds?.Count is not null)
                {
                    await presenceHub.Clients.Clients(connectionIds).SendAsync("NewMessageReceived", 
                        new{ username = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            messageRepository.AddMessage(message);

            if (await messageRepository.SaveAllAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDTO>(message));
            }
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var username = Context.User?.GetUserName() ?? throw new HubException("Cannot get username.");
            var group = await messageRepository.GetMessageGroupAsync(groupName);
            var connection = new Connection() { ConnectionId = Context.ConnectionId, Username = username };

            if (group is null)
            {
                group = new Group()
                {
                    Name = groupName,
                };
                messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await messageRepository.SaveAllAsync()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group =await messageRepository.GetGroupForConneciton(Context.ConnectionId);
            var connection = group?.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (connection is not null && group is not null)
            {
                messageRepository.RemoveConnection(connection);
                if(await messageRepository.SaveAllAsync()) return group;
            }
            throw new HubException("Failed to remove from group");
        }
        private static string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) <= 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
