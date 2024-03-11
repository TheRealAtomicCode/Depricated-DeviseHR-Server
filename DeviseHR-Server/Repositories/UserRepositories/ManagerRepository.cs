using DeviseHR_Server.Models;
using System.ComponentModel.Design;
using DeviseHR_Server.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DeviseHR_Server.Handlers.SqlExceptionHandlers;
using static DeviseHR_Server.DTOs.RequestDTOs.ManagerUserRequests;

namespace DeviseHR_Server.Repositories.UserRepository
{
    public class ManagerRepository
    {
        public static async Task<string?> InsertNewUser(NewUser newUser, int myId, int companyId, int myUserType)
        {
            if (newUser.AnnualLeaveYearStartDate == null) throw new Exception("Unablable Company annual leave year start date");
            string? verificationCode = null;

            User user = new User
            {
                CompanyId = companyId,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                DateOfBirth = newUser.DateOfBirth,
                UserType = newUser.UserType,
                AddedByUser = myId,
                AddedByOperator = 0,
                RoleId = newUser.UserType == 2 ? newUser.RoleId : null,
                AnnualLeaveStartDate = newUser.AnnualLeaveYearStartDate.Value,
                RefreshTokens = []
            };

            using (var db = new DeviseHrContext())
            {
                // ! force add user as employee if manager
                if (myUserType == 2)
                {
                    newUser.UserType = 3;
                    newUser.RoleId = null;
                }

                db.Users.Add(user); // how to check if the email already exists error handle

                if (myUserType == 2)
                {
                    Hierarchy hierarchy = new Hierarchy
                    {
                        ManagerId = myId,
                        SubordinateId = user.Id
                    };

                    db.Hierarchies.Add(hierarchy);
                }

                // update verification code
                if (newUser.RegisterUser == true) verificationCode = StringGeneration.GenerateSixDigitString();
                if (newUser.RegisterUser == true) user.VerificationCode = verificationCode!;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    UserSqlExceptionHandler.InsertUserSqlExceptionHandler(ex);
                }
            }

            return verificationCode;
        }

        public static async Task<User> UpdateUserVerificationCodeBuId(int userId, int myId, int companyId, int userType)
        {
            using (var db = new DeviseHrContext())
            {
                User? user = null;

                if (userType == 1)
                {
                    user = await db.Users
                        .FirstOrDefaultAsync(u => u.Id == userId && u.CompanyId == companyId);
                }
                else if (userType == 2)
                {
                    user = await db.Users
                        .Include(u => u.HierarchySubordinates)
                        .FirstOrDefaultAsync(u => u.Id == userId
                            && u.CompanyId == companyId
                            && u.HierarchySubordinates.Any(sub => sub.SubordinateId == userId && sub.ManagerId == myId));
                }

                if (user == null) throw new Exception("Failed to locate user");

                // update verification code
                if (user.IsVerified) throw new Exception("User already Registered");

                string verificationCode = StringGeneration.GenerateSixDigitString();

                await db.SaveChangesAsync();

                return user;
            }
        }




    }




}

