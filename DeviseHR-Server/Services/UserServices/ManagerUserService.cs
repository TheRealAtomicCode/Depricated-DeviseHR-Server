using DeviseHR_Server.Common;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories;
using DeviseHR_Server.Repositories.UserRepository;
using DeviseHR_Server.Services.EmailServices;
using System.ComponentModel.Design;
using static DeviseHR_Server.DTOs.RequestDTOs.ManagerUserRequests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviseHR_Server.Services.UserServices
{
    public class ManagerUserService
    {
        public static async Task<NewUser> AddUser(NewUser newUser, int myId, int companyId, int myUserType, DateOnly companyAnnualLeaveDate)
        {
            if (newUser.UserType != 1 && newUser.UserType != 2 && newUser.UserType != 3) throw new Exception("Invalid User Permission");
            // * check if manager, then make sure user being added is employee
            if (myUserType == 2 && newUser.UserType != 3) throw new Exception("managers can only add users with an employee role");
            // * check if manager has a role
            if (newUser.UserType == 2 && (newUser.RoleId == null || newUser.RoleId <= 0)) throw new Exception("Managers must have a role assigned");

            Filters.filterNewUserData(newUser);

            if (newUser.AnnualLeaveYearStartDate == null) newUser.AnnualLeaveYearStartDate = companyAnnualLeaveDate;

            string? verificationCode = await ManagerRepository.InsertNewUser(newUser, myId, companyId, myUserType);

            if (verificationCode != null)
            {
                await SendNotificationService.SendUserRegistration(newUser.Email, newUser.FirstName, newUser.LastName, verificationCode);
            }
            return newUser;
        }

        public static async Task SendRegistration(int userId, int myId, int companyId, int userType)
        {
            User user = await ManagerRepository.UpdateUserVerificationCodeBuId(userId, myId, companyId, userType);

            if (user.VerificationCode != null)
            {
                await SendNotificationService.SendUserRegistration(user.Email, user.FirstName, user.LastName, user.VerificationCode);
            }
        }

    }
}
