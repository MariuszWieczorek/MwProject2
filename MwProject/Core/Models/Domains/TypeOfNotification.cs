using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Models.Domains
{
    public class TypeOfNotification
    {
        public TypeOfNotification()
        {
            Notifications = new Collection<Notification>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}
