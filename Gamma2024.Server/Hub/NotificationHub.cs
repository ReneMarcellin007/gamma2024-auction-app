using Gamma2024.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Gamma2024.Server.Hub
{
    public interface INotificationHub
    {
        Task ReceiveNotification(object value);
    }

    [Authorize]
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private static readonly Dictionary<string, HashSet<string>> _userConnections = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier!;
            var connectionId = Context.ConnectionId;

            lock (_userConnections)
            {
                if (!_userConnections.ContainsKey(userId))
                {
                    _userConnections[userId] = [];
                }
                _userConnections[userId].Add(connectionId);
            }


            await base.OnConnectedAsync();
        }

        public async Task SendToUser(Notification notification)
        {
            try
            {
                await Clients.User(notification.ApplicationUserId).SendAsync("ReceiveNotification", notification);
            }
            catch
            {
                return;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            var userId = Context.UserIdentifier!;
            var connectionId = Context.ConnectionId;

            lock (_userConnections)
            {
                if (_userConnections.ContainsKey(userId))
                {
                    _userConnections[userId].Remove(connectionId);
                    if (_userConnections[userId].Count == 0)
                    {
                        _userConnections.Remove(userId);
                    }
                }
            }

            await base.OnDisconnectedAsync(ex);
        }

        public HashSet<string> GetUserConnections(string userId)
        {
            lock (_userConnections)
            {
                return _userConnections.GetValueOrDefault(userId, new HashSet<string>());
            }
        }

    }
}