using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MwProject.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IApplicationDbContext _context;
        public NotificationRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Notification> GetNotifications()
        {
            return _context.Notifications.ToList();
        }

        public void AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
        }

        public Notification GetNotification(int id)
        {
            return _context.Notifications.Single(x => x.Id == id);
        }

        public void UpdateNotification(Notification notification)
        {
            var notificationToUpdate = _context.Notifications.Single(x => x.Id == notification.Id);
            notificationToUpdate.Content = notificationToUpdate.Content;
        }

        public void DeleteNotification(int id)
        {
            var notificationToDelete = _context.Notifications.Single(x => x.Id == id);
            _context.Notifications.Remove(notificationToDelete);
        }

        public Notification NewNotification()
        {
            return new Notification
            {
                TimeOfNotification = DateTime.Now
            };
        }

        public void ConfirmProjectNotification(int projectId, int id, string userId)
        {
            var notificationToConfirm = _context.Notifications.Single(x => x.ProjectId == projectId && x.Id == id);
            notificationToConfirm.Confirmed = true;
            notificationToConfirm.ConfirmedDate = DateTime.Now;
        }
    }
}
