using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.ContractRepositories;
using DeviseHR_Server.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DeviseHR_Server.Services.ContractServices
{
    public static class EmployeeContract
    {

        //public static async Task<List<LeaveYear>> GetLeaveYearService(int userId, int myId, int companyId, int userType, bool enableShowEmployees)
        //{
        //    bool checkIfSubordinate = true;
        //    if ((userType == 2 && enableShowEmployees == true) || userType == 1) checkIfSubordinate = false;

        //    if (userType == 3 && userId != myId) throw new Exception("You can not view other user profiles");

        //    var db = new DeviseHrContext();

        //    if (checkIfSubordinate)
        //    {
        //        bool isRelated = await UserRelationsRepo.IsRelated(myId, userId);

        //        if (isRelated) throw new Exception("You can not view this users profle");

        //    }
            
        //    List<LeaveYear> leaveYears = await ContractRepo.GetLeaveYearsRepo(db, userId, companyId);

        //    await ContractRepo.AddMissingLeaveYearRepo(db, leaveYears, userId, companyId);

        //    return leaveYears;
        //}
    }
}
