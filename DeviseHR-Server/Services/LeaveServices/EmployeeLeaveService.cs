using DeviseHR_Server.DTOs.RequestDTOs;

namespace DeviseHR_Server.Services.LeaveServices
{
    public class EmployeeLeaveService
    {

        public static async Task RequestLeaveService(AddAbsenceRequest newAbsence, int myId, int companyId, int userType)
        {
            if(newAbsence.UserId == myId)
            {
                if (userType != 1)
                {
                    // request
                }
                else 
                { 
                    // get my managers
                    // add or request
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
                    // get my subordinates
                    // add or Error
                }
                else
                {
                    // error
                }

             
            }


        }
    }
}
