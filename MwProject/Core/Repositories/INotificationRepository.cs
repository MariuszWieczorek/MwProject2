using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetNotifications();
        void AddNotification(Notification notification);
        Notification GetNotification(int id);
        Notification NewNotification();
        void UpdateNotification(Notification notification);
        void DeleteNotification(int id);

    }
}
