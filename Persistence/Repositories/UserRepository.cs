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

        public ApplicationUser GetUser(string id)
        {
            return _context.Users.Single(x => x.Id == id);

        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}
