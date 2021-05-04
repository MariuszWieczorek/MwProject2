using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;

namespace MwProject.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

          public ApplicationUser GetUser(string id)
        {
            return _unitOfWork.UserRepository.GetUser(id);
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _unitOfWork.UserRepository.GetUsers();
        }
    }
}
