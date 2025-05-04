
using Data.Context;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public interface INotificationDissmissRepository : IBaseRepository<NotificationDismissedEntity, Notification>
{
}

public class NotificationDissmissRepository(DataContext context) : BaseRepository<NotificationDismissedEntity, Notification>(context)
{
}