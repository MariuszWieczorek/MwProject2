using MwProject.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface IUserService
    {
        ApplicationUser GetUser(string id);
        void UpdateUser(ApplicationUser user);
        void DeleteUser(string id);
        IEnumerable<ApplicationUser> GetUsers();
    }
}
