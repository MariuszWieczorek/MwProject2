using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Models.Filters;
using MwProject.Core.Repositories;
using MwProject.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }


        public void DeleteUser(string id)
        {
            var userToDelete = _context.Users.Single(x => x.Id == id);
            _context.Users.Remove(userToDelete);
        }

        public void ExportUsersToExcel(IEnumerable<ApplicationUser> users)
        {
            throw new NotImplementedException();
        }

        public int GetNumberOfRecords(UsersFilter usersFilter)
        {
            int numberOfRecords = 0;
            var users = _context.Users
                 .AsQueryable();

            // filtrowanie
            if (usersFilter != null)
            {
                if (!string.IsNullOrWhiteSpace(usersFilter.UserName))
                    users = users.Where(x => x.UserName.Contains(usersFilter.UserName));

          
            }
            numberOfRecords = users.Count();
            return numberOfRecords;
        }

        public ApplicationUser GetUser(string id)
        {
            return _context.Users.Single(x => x.Id == id);

        }

        public IEnumerable<ApplicationUser> GetUsers(UsersFilter usersFilter, PagingInfo pagingInfo)
        {
            int numberOfRecords = 0;
            var users = _context.Users
                        .OrderBy(x => x.UserName)
                        .AsQueryable();

            // filtrowanie
            if (usersFilter != null)
            {
                if (!string.IsNullOrWhiteSpace(usersFilter.UserName))
                    users = users.Where(x => x.UserName.Contains(usersFilter.UserName));

       
            }

            numberOfRecords = users.Count();

            // stronicowanie
            if (pagingInfo != null)
            {
                pagingInfo.TotalItems = numberOfRecords;

                if (pagingInfo.ItemsPerPage > 0 && pagingInfo.TotalItems > 0)
                    users = users
                        .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.ItemsPerPage)
                        .Take(pagingInfo.ItemsPerPage);
            }

            return users
                .ToList();
        }

    
        public void UpdateUser(ApplicationUser user)
        {
            var userToUpdate = _context.Users.Single(x => x.Id == user.Id);
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Possition = user.Possition;
            userToUpdate.PhoneNumber = user.PhoneNumber;
            userToUpdate.Email = user.Email;

            userToUpdate.CanAcceptProject = user.CanAcceptProject;
            userToUpdate.CanConfirmProject = user.CanConfirmProject;
            userToUpdate.CanConfirmRequest = user.CanConfirmRequest;
            userToUpdate.CanConfirmCalculations = user.CanConfirmCalculations;
            userToUpdate.CanConfirmEstimatedSales = user.CanConfirmEstimatedSales;
            userToUpdate.CanConfirmQualityRequirements = user.CanConfirmQualityRequirements;
            userToUpdate.CanConfirmEconomicRequirements = user.CanConfirmEconomicRequirements;
            userToUpdate.CanConfirmTechnicalProperties = user.CanConfirmTechnicalProperties;
            userToUpdate.CanConfirmGeneralRequirements = user.CanConfirmGeneralRequirements;
            userToUpdate.CanConfirmProjectTeam = user.CanConfirmProjectTeam;
            userToUpdate.CanSetProjectManager = user.CanSetProjectManager;

            userToUpdate.AdminRights = user.AdminRights;
            userToUpdate.SuperAdminRights = user.SuperAdminRights;
            userToUpdate.CanModifyProject = user.CanModifyProject;
            userToUpdate.CanSeeAllProject = user.CanSeeAllProject;

            userToUpdate.CanEditCalculations = user.CanEditCalculations;
            userToUpdate.CanEditEstimatedSales = user.CanEditEstimatedSales;
            userToUpdate.CanEditQualityRequirements = user.CanEditQualityRequirements;
            userToUpdate.CanEditEconomicRequirements = user.CanEditEconomicRequirements;
            userToUpdate.CanEditTechnicalProperties = user.CanEditTechnicalProperties;
            userToUpdate.CanEditGeneralRequirements = user.CanEditGeneralRequirements;
            userToUpdate.CanEditProjectTeam = user.CanEditProjectTeam;
            userToUpdate.ReferenceNumber = user.ReferenceNumber;

            userToUpdate.EmailConfirmed = user.EmailConfirmed;
            userToUpdate.IsManager = user.IsManager;
            userToUpdate.NewProjectEmailNotification = user.NewProjectEmailNotification;
            userToUpdate.ManagerId = user.ManagerId;

        }


        public async Task AddUser(ApplicationUser applicationUser)
        {
            string password = "Projekty2021$";

            var result = await _userManager.CreateAsync(applicationUser, password);
            var x = result;
        }

        public ApplicationUser NewUser()
        {
            return new ApplicationUser();
        }

        public bool UserExist(string id)
        {
            var numberOfUsers = _context.Users.Where(x => x.Id == id).Count();
            return numberOfUsers == 0;
        }

    }
}
