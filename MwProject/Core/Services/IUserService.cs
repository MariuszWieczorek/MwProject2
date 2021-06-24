using Microsoft.AspNetCore.Identity;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Filters;
using MwProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Core.Services
{
    public interface IUserService
    {
        IEnumerable<ApplicationUser> GetUsers(UsersFilter usersFilter, PagingInfo pagingInfo);
        int GetNumberOfRecords(UsersFilter usersFilter);
        ApplicationUser GetUser(string id);
        void UpdateUser(ApplicationUser user);
        void DeleteUser(string id);
        void ExportUsersToExcel(IEnumerable<ApplicationUser> users);
        Task ImportUsersFromExcel();
        void RepairUsers();
        Task<IdentityResult> ResetPassword(string id);
    }
}
