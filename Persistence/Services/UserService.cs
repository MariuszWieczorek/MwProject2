using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace MwProject.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public void DeleteUser(string id)
        {
            _unitOfWork.UserRepository.DeleteUser(id);
            _unitOfWork.Complete();
        }

        public ApplicationUser GetUser(string id)
        {
            return _unitOfWork.UserRepository.GetUser(id);
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _unitOfWork.UserRepository.GetUsers().OrderBy(x=>x.UserName);
        }

        public async Task<IdentityResult> ResetPassword(string id)
        {
            //            var user = _unitOfWork.UserRepository.GetUser(id);
            //System.Security.Claims.ClaimsPrincipal claims

            //var user = await _userManager.GetUserAsync(claims);
            string password = "Projekty2021$";


            var user = await _userManager.FindByIdAsync(id);
            var token  = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, password); 

            var result = await _userManager.ResetPasswordAsync(user, token, password);
            
            if (result.Succeeded)
                await _userManager.UpdateAsync(user);

            return result;
//            await _userManager.RemovePasswordAsync(user);
  //          await _userManager.AddPasswordAsync(user, password);


            /*
            .ContinueWith(t => 
            { 
                var res = t.Result;
                if (res.Succeeded)
                    a = 1;
                else
                    a = 2;
            });
            */


        }

        public void UpdateUser(ApplicationUser user)
        {
            _unitOfWork.UserRepository.UpdateUser(user);
            _unitOfWork.Complete();
        }
    }
}
