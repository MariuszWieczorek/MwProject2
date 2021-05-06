using Microsoft.EntityFrameworkCore;
using MwProject.Core;
using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
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
        public UserRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteUser(string id)
        {
            var userToDelete = _context.Users.Single(x => x.Id == id);
            _context.Users.Remove(userToDelete);
        }

        public ApplicationUser GetUser(string id)
        {
            return _context.Users.Single(x => x.Id == id);

        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void UpdateUser(ApplicationUser user)
        {
            var userToUpdate = _context.Users.Single(x => x.Id == user.Id);
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Possition = user.Possition;

            userToUpdate.CanAcceptProject = user.CanAcceptProject;
            userToUpdate.CanConfirmProject = user.CanConfirmProject;
            userToUpdate.CanConfirmCalculations = user.CanConfirmCalculations;
            userToUpdate.CanConfirmEstimatedSales = user.CanConfirmEstimatedSales;
            userToUpdate.CanConfirmQualityRequirements = user.CanConfirmQualityRequirements;
            userToUpdate.CanConfirmEconomicRequirements = user.CanConfirmEconomicRequirements;
            userToUpdate.CanConfirmTechnicalProperties = user.CanConfirmTechnicalProperties;
            userToUpdate.AdminRights = user.AdminRights;
            userToUpdate.CanModifyProject = user.CanModifyProject;

        }
    }
}
