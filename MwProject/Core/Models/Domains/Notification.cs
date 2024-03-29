﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int TypeOfNotificationId { get; set; }
        public TypeOfNotification TypeOfNotification { get; set; }
        public DateTime TimeOfNotification { get; set; }
        public string Content { get; set; }
        
        public string Link { get; set; }

        public string ToDo { get; set; }
        public bool Sent { get; set; }
        public bool Confirmed { get; set; }
        public DateTime? ConfirmedDate { get; set; }
    }
}
