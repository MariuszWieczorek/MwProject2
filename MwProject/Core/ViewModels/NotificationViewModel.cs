using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class NotificationViewModel
    {
        public string Heading { get; set; }
        public Notification Notification { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}
