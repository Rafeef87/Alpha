
using Data.Entities;
using Data.Repositories;
using Domain.Models;
using Domain.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Business.Services;

public interface INotificationService
{
    Task DismissNotificationAsync(string userId, int notificationId);
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
    Task SendNotificationAsync(string userId, Notification notification);
}

public class NotificationService(INotificationRepository notificationRepository, INotificationDissmissRepository notificationDissmissRepository, IHubContext<NotificationHub> hubContext) : INotificationService
{
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly INotificationDissmissRepository _notificationDissmissRepository = notificationDissmissRepository;
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;
    public async Task SendNotificationAsync(string userId, Notification notification)
    {
        // Map Domain.Models.Notification to Data.Entities.NotificationEntity
        var notificationEntity = new NotificationEntity
        {
            Id = notification.Id.ToString(),
            NotificationTypeId = notification.Type != null ? int.Parse(notification.Type) : 0,
            Message = notification.Message,
            Image = notification.Image!,
            Created = notification.Created
        };

        // Add notification entity to the database
        var result = await _notificationRepository.AddAsync(notificationEntity);
        if (!result.Succeeded)
            throw new Exception($"Kunde inte skapa notifikation: {result.Error}");

        // Send the notification via SignalR
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
    {
        var result = await _notificationRepository.GetAllAsync(
            orderByDescending: true,
            storBy: n => n.Created

        );

        return result.Result ?? new List<Notification>();
    }

    public async Task DismissNotificationAsync(string userId, int notificationId)
    {
        var dismiss = new NotificationDismissedEntity
        {
            UserId = userId,
            NotificationId = notificationId.ToString(),
            Notification = new NotificationEntity
            {
                Id = notificationId.ToString()
            }
        };

        var result = await _notificationDissmissRepository.AddAsync(dismiss);
        if (!result.Succeeded)
            throw new Exception($"Kunde inte dölja notifikation: {result.Error}");
    }
}
