using Microsoft.AspNetCore.SignalR;

namespace finance_control.Services.SignalR
{
    public class NotificationHub : Hub
    {     
        private static readonly Dictionary<string, string> _connections = new();

        public Task RegisterUser(string userId)
        {
            _connections[userId] = Context.ConnectionId;
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var item = _connections.FirstOrDefault(kvp => kvp.Value == Context.ConnectionId);
            if (!string.IsNullOrEmpty(item.Key))
                _connections.Remove(item.Key);

            return base.OnDisconnectedAsync(exception);
        }

        public static string? GetConnectionId(string userId)
        {
            return _connections.TryGetValue(userId, out var connId) ? connId : null;
        }
    }


}
