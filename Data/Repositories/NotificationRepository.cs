
using Data.Context;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public interface INotificationRepository : IBaseRepository<NotificationEntity, Notification>
{
}
public class NotificationRepository(DataContext context) : BaseRepository<NotificationEntity, Notification>(context), INotificationRepository
{

}

