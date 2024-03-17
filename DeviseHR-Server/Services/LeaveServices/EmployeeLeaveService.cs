using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.AbsenceRepositories;
using DeviseHR_Server.Repositories.RoleRepositories;

namespace DeviseHR_Server.Services.LeaveServices
{
    public class EmployeeLeaveService
    {

        public static async Task<Absence> RequestAbsenceService(AddAbsenceRequest newAbsence, int myId, int companyId, int userType)
        {

            DateOnly startDate = DateOnly.Parse(newAbsence.StartDate);
            DateOnly endDate = DateOnly.Parse(newAbsence.EndDate);
            
            if(newAbsence.UserId == myId)
            {
                if (userType == 1)
                {
                    // check if i have a manager
                    bool hasManager = await UserRelationsRepository.HasManager(myId);
                    
                    if(hasManager)
                    {
                        // request
                        return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, myId, companyId, false);
                    }
                    else
                    {
                        // add
                        return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, myId, companyId, true);
                    }
                }
                else
                {
                    // request
                    return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, myId, companyId, false);
                }
             

            }
            else
            {
                if(userType == 1)
                {
                    return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, myId, companyId, true);
                }
                else if(userType == 2)
                {
                    bool isSubordinate = await UserRelationsRepository.IsRelated(myId, newAbsence.UserId);
                    
                    if(isSubordinate)
                    {
                        return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, myId, companyId, true);
                    }
                    else
                    {
                        // error
                        throw new Exception("Can not add absence for user who is not your subordinate");
                    }
                }
                else
                {
                    // error
                    throw new Exception("You do not have permissions to add absences for other users");
                }


             
            }


        }
    }
}
