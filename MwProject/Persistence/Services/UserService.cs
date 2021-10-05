using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MwProject.Core;
using MwProject.Core.Services;
using MwProject.Core.Models.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using MwProject.Core.Models.Filters;
using MwProject.Core.ViewModels;
using ClosedXML.Excel;
using MwProject.Helpers;

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

        public IEnumerable<ApplicationUser> GetUsers(UsersFilter usersFilter, PagingInfo pagingInfo)
        {
            return _unitOfWork.UserRepository.GetUsers(usersFilter, pagingInfo).OrderBy(x => x.UserName);
        }

        public int GetNumberOfRecords(UsersFilter usersFilter)
        {
            return _unitOfWork.UserRepository.GetNumberOfRecords(usersFilter);
        }

        public void UpdateUser(ApplicationUser user)
        {
            _unitOfWork.UserRepository.UpdateUser(user);
            _unitOfWork.Complete();
        }

        #region Excel


        public ApplicationUser NewUser()
        {
            return _unitOfWork.UserRepository.NewUser();
        }

        public void ExportUsersToExcel(IEnumerable<ApplicationUser> users)
        {
            // PM> Install-Package ClosedXML

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");

                int row = 1;
                foreach (var user in users)
                {
                    worksheet.Cell($"A{row}").RichText.AddText(user.Id.ToString()).SetFontColor(XLColor.Blue).SetBold();
                    worksheet.Cell(row, "B").RichText.AddText(user.ReferenceNumber).SetFontColor(XLColor.Black).SetBold();
                    worksheet.Cell(row, "C").Value = user.FirstName;
                    worksheet.Cell(row, "D").Value = user.LastName;



                    // Add the text parts
                    /*
                    var cell = worksheet.Cell(row, "D");
                    cell.RichText
                      .AddText("Hello").SetFontColor(XLColor.Red)
                      .AddText(" BIG ").SetFontColor(XLColor.Blue).SetBold()
                      .AddText("World").SetFontColor(XLColor.Red);
                    */
                    row++;
                }

                // worksheet.Cell($"A{row}").FormulaA1 = "=MID(A1, 7, 5)";
                workbook.SaveAs("ListOfUsers.xlsx");
            }
        }

        public void RepairUsers()
        {
            var users = _unitOfWork.UserRepository
                .GetUsers(new UsersFilter(), new PagingInfo());

            foreach (var user in users)
            {
                var userToUpdate = _unitOfWork.UserRepository.GetUser(user.Id);
                userToUpdate.UserName = user.UserName.StripText().ToLower();
                userToUpdate.Email = user.Email.StripText().ToLower();
                userToUpdate.NormalizedEmail = user.NormalizedEmail.StripText().ToUpper();
                userToUpdate.NormalizedUserName = user.NormalizedUserName.StripText().ToUpper();
                _unitOfWork.Complete();
            }

        }

        public async Task ImportUsersFromExcel()
        {
            string fileName = "ListOfUsersToImport.xlsx";
            using (var excelWorkbook = new XLWorkbook(fileName))
            {
                var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();

                foreach (var dataRow in nonEmptyDataRows)
                {
                    //for row number check
                    if (dataRow.RowNumber() >= 2 && dataRow.RowNumber() <= 450)
                    {
                        //to get column # 3's data
                        string referenceNumber = dataRow.Cell(1).Value.ToString().Trim();
                        string lastName = dataRow.Cell(2).Value.ToString().Trim();
                        string firstName = dataRow.Cell(3).Value.ToString().Trim();
                        string areaAbbrev = (string)dataRow.Cell(5).Value;

                        var usersInDatabase = _unitOfWork.UserRepository
                                        .GetUsers(new UsersFilter(), new PagingInfo())
                                        .Where(x => x.FirstName == firstName && x.LastName == lastName && x.ReferenceNumber == referenceNumber);

                        if (!usersInDatabase.Any())
                        {

                            string emailAddress = $"{firstName}.{lastName}@kabat.pl".ToLower().StripText();
                            string password = "Projekty2021$";

                            var user = new ApplicationUser
                            {
                                UserName = emailAddress,
                                Email = emailAddress,
                                FirstName = firstName,
                                LastName = lastName,
                                ReferenceNumber = referenceNumber,
                                Possition = "a"
                            };

                            var result = _userManager.CreateAsync(user, password);
                            var x = result;

                        }

                    }
                }
            }
        }

        public async Task AddUser(ApplicationUser applicationUser)
        {
            await _unitOfWork.UserRepository.AddUser(applicationUser);
        }

        public bool UserExist(string id)
        {
            return _unitOfWork.UserRepository.UserExist(id);
        }
        public async Task<IdentityResult> ResetPassword(string id)
        {

            string password = "Projekty2021$";
            var user = await _userManager.FindByIdAsync(id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, password);
        }





        #endregion
    }
}
