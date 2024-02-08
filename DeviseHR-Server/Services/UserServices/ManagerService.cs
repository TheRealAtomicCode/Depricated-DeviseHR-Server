using DeviseHR_Server.Common;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories;
using static DeviseHR_Server.DTOs.RequestDTOs.ManagerRequests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviseHR_Server.Services.UserServices
{
    public class ManagerService
    {
        public static async Task<bool> AddUser(NewUser newUser, int myId, int companyId, int userType, DateOnly companyAnnualLeaveDate)
        {
            DateTime defaultAnnualLeaveStartDate = new DateTime(1970, 1, 1).Date;
            if (newUser.UserType != 1 || newUser.UserType != 2 || newUser.UserType != 3) throw new Exception("Invalid User Permission");
            // * check if manager, then make sure user being added is employee
            if (userType == 2 && newUser.UserType != 3) throw new Exception("managers can only add users with an employee role");
            // * check if manager has a role
            if (newUser.UserType == 2 && (newUser.RoleId != null || newUser.RoleId <= 0)) throw new Exception("Managers must have a role assigned");

            TrimmedUserData trimmedNewUser = Filters.filternewUserData(newUser.FirstName, newUser.LastName, newUser.Email);

            if (newUser.AnnualLeaveYearStartDate == null) newUser.AnnualLeaveYearStartDate = companyAnnualLeaveDate;

            AddedUser addedUser = ManagerRepository.AddUser();

            return true;
        }
        
    }
}
