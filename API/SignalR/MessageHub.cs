using API.DTOs.messages;
using API.Entities;
using API.Extentions;
using API.Interfaces;
using API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub(IUnitOfWork _unitOfWork, IMapper _mapper, IHubContext<PresenceHub> presenceHub) : Hub
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

            var messages = await _unitOfWork.MessageRepository.GetMessageThread(Context.User.GetUserName(), otherUser!);
            // In GetMessageThread() we check if Recipient open connection and read its received messages,
            // we change DateRead from null to DateTime.UtcNow,
            // so we must check if there any changes change-tracker found and store it into our database.
            if (_unitOfWork.HasChanges()) await _unitOfWork.CompleteAsync();
           
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

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(messageDTO.RecipientUserName);
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
            var group = await _unitOfWork.MessageRepository.GetMessageGroupAsync(groupName);

            // if recipient user is already in the chatroom
            if (group is not null && group.Connections.Any(connection => connection.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connectionIdsForRecipient = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
                if (connectionIdsForRecipient is not null && connectionIdsForRecipient?.Count is not null)
                {
                    await presenceHub.Clients.Clients(connectionIdsForRecipient).SendAsync("NewMessageReceived", 
                        new{ username = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.CompleteAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDTO>(message));
            }
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var username = Context.User?.GetUserName() ?? throw new HubException("Cannot get username.");
            var group = await _unitOfWork.MessageRepository.GetMessageGroupAsync(groupName);
            var connection = new Connection() { ConnectionId = Context.ConnectionId, Username = username };

            if (group is null)
            {
                group = new Group()
                {
                    Name = groupName,
                };
                _unitOfWork.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _unitOfWork.CompleteAsync()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group =await _unitOfWork.MessageRepository.GetGroupForConneciton(Context.ConnectionId);
            var connection = group?.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (connection is not null && group is not null)
            {
                _unitOfWork.MessageRepository.RemoveConnection(connection);
                if(await _unitOfWork.CompleteAsync()) return group;
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
