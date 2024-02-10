using DeviseHR_Server.Models;
using static DeviseHR_Server.DTOs.RequestDTOs.ManagerRequests;
using System.ComponentModel.Design;
using DeviseHR_Server.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DeviseHR_Server.Handlers.SqlExceptionHandlers;

namespace DeviseHR_Server.Repositories.UserRepository
{
    public class ManagerRepository
    {
        public static async Task<string?> InsertNewUser(NewUser newUser, int myId, int companyId)
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
                RoleId = newUser.RoleId == 2 ? newUser.RoleId : null,
                AnnualLeaveStartDate = newUser.AnnualLeaveYearStartDate.Value,
                RefreshTokens = []
            };

            using (var db = new DeviseHrContext())
            {
                db.Users.Add(user); // how to check if the email already exists error handle

                if (newUser.UserType != 1)
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

    

    }
    
        


}

