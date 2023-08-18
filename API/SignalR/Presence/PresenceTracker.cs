namespace API.SignalR.Presence
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> _onlineUsers = new();

        public Task<bool> UserConnected(string userName, string connectionId)
        {
            bool isOnline = false;

            lock (_onlineUsers)
            {
                lock (_onlineUsers)
                {
                    if (_onlineUsers.ContainsKey(userName))
                    {
                        _onlineUsers[userName].Add(connectionId);
                    }
                    else
                    {
                        _onlineUsers.Add(userName, new List<string> { connectionId });
                        isOnline = true;    
                    }
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string userName, string connectionId)
        {
            bool isOffline = false;

            lock (_onlineUsers)
            {
                if (!_onlineUsers.ContainsKey(userName))
                    return Task.FromResult(isOffline);

                _onlineUsers[userName].Remove(connectionId);

                if (_onlineUsers[userName].Count == 0)
                {
                    _onlineUsers.Remove(userName);
                    isOffline = true;
                }
            }

            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;

            lock (_onlineUsers)
            {
                onlineUsers = _onlineUsers.OrderBy(x => x.Key)
                    .Select(x => x.Key)
                    .ToArray();
            }

            return Task.FromResult(onlineUsers);
        }

        public Task<List<string>> GetConnectionsForUser(string userName)
        {
            List<string> connectiondIds;

            lock (_onlineUsers)
            {
                connectiondIds = _onlineUsers.GetValueOrDefault(userName);
            }

            return Task.FromResult(connectiondIds);
        }
    }
}
