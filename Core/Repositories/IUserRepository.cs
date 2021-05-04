using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser GetUser(string id);
        IEnumerable<ApplicationUser> GetUsers();
    }
}
