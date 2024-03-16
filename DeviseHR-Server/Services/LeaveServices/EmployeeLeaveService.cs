using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Repositories.RoleRepositories;

namespace DeviseHR_Server.Services.LeaveServices
{
    public class EmployeeLeaveService
    {

        public static async Task RequestLeaveService(AddAbsenceRequest newAbsence, int myId, int companyId, int userType)
        {
            if(newAbsence.UserId == myId)
            {
                if (userType == 1)
                {
                    // check if i have a manager
                    bool hasManager = await UserRelationsRepository.HasManager(myId);
                    
                    if(hasManager)
                    {
                        // request
                    }
                    else
                    {
                        // add
                    }
                }
                else
                {
                    // request
                }
             

            }
            else
            {
                if(userType == 1)
                {
                    // add
                }
                else if(userType == 2)
                {
                    bool isSubordinate = await UserRelationsRepository.IsRelated(myId, newAbsence.UserId);
                    
                    if(isSubordinate)
                    {
                        // add
                    }
                    else
                    {
                        // error
                    }
                }
                else
                {
                    // error
                }


             
            }


        }
    }
}
