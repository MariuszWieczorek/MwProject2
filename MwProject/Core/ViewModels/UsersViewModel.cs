using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public UsersFilter UsersFilter { get; set; }
        public int NumberOfRecords { get; set; }
    }
}
