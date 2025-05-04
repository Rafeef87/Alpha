using Microsoft.AspNetCore.SignalR;

namespace Domain.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId); // grupper per userId
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    // Server skickar notis till en viss användare
    public async Task SendNotificationToUser(string userId, object notification)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", notification);
    }

    // Server skickar till en viss "grupp" (t.ex. admin)
    public async Task SendNotificationToRoleGroup(string groupName, object notification)
    {
        await Clients.Group(groupName).SendAsync("AdminReceiveNotification", notification);
    }
}

