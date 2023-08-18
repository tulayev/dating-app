using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR.Presence
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _presenceTracker;

        public PresenceHub(PresenceTracker presenceTracker)
        {
            _presenceTracker = presenceTracker;
        }

        public override async Task OnConnectedAsync()
        {
            string userName = Context.User.GetUserName();

            bool isOnline = await _presenceTracker.UserConnected(userName, Context.ConnectionId);
            
            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", userName);

            var currentOnlineUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentOnlineUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userName = Context.User.GetUserName();

            bool isOffline = await _presenceTracker.UserDisconnected(userName, Context.ConnectionId);

            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", userName);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
