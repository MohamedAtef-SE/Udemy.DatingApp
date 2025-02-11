using System.Collections.Concurrent;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly ConcurrentDictionary<string,List<string>> OnlineUsers = new();

        public Task<bool> UserConnected(string username,string connectionId)
        {
            OnlineUsers.AddOrUpdate(username, new List<string>() { connectionId },
                (key,existingList) =>
                {
                    existingList.Add(connectionId);
                    return existingList;
                });
            var isOnline = OnlineUsers.TryGetValue(username, out var connections) && connections.Count > 0;
            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            var isOffline = false;
            if (OnlineUsers.TryGetValue(username,out var connections))
            {
                OnlineUsers[username].Remove(connectionId);
                //connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    isOffline =  OnlineUsers.TryRemove(username,out _);
                }
            }
            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers = OnlineUsers.Keys.OrderBy(username => username).ToArray();

            return Task.FromResult(onlineUsers);
        }

        public static Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds = new List<string>();

            if (OnlineUsers.TryGetValue(username, out var connections))
            {
                connectionIds = connections.ToList();
            }

            return Task.FromResult(connectionIds);
        }
    }
}
