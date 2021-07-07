using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetNotifications();
        void AddNotification(Notification notification);
        Notification GetNotification(int id);
        Notification NewNotification();
        void UpdateNotification(Notification notification);
        void DeleteNotification(int id);
        void ConfirmProjectNotification(int projectId, int id, string userId);
    }
}
