using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Notification> GetNotifications()
        {
            return _unitOfWork.NotificationRepository.GetNotifications();
        }

        public void AddNotification(Notification notification)
        {
            _unitOfWork.NotificationRepository.AddNotification(notification);
            _unitOfWork.Complete();
        }

        public Notification GetNotification(int id)
        {
            return _unitOfWork.NotificationRepository.GetNotification(id);
        }

        public void UpdateNotification(Notification notification)
        {
            _unitOfWork.NotificationRepository.UpdateNotification(notification);
            _unitOfWork.Complete();
        }

        public void DeleteNotification(int id)
        {
            _unitOfWork.NotificationRepository.DeleteNotification(id);
            _unitOfWork.Complete();
        }

        public Notification NewNotification()
        {
            return _unitOfWork.NotificationRepository.NewNotification();
        }
    }
}
