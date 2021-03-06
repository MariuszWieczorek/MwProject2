using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class UserViewModel
    {
        public string Heading { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public IEnumerable<ApplicationUser> Managers { get; set; }
    }
}
