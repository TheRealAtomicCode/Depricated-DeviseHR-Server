//using DeviseHR_Server.DTOs.RequestDTOs;
//using DeviseHR_Server.Models;
//using DeviseHR_Server.Repositories.AbsenceRepositories;
//using DeviseHR_Server.Repositories.GetterRepos;

//namespace DeviseHR_Server.Services.LeaveServices
//{
//    public class EmployeeLeaveService
//    {

//        public static async Task<Absence> RequestAbsenceService(AddAbsenceRequest newAbsence, int myId, int companyId, int userType)
//        {

//            DateOnly startDate = DateOnly.Parse(newAbsence.StartDate);
//            DateOnly endDate = DateOnly.Parse(newAbsence.EndDate);

//            int startHour = newAbsence.StartTime / 60;
//            int startMinute = newAbsence.StartTime % 60;
//            TimeOnly startTime = new TimeOnly(startHour, startMinute);

//            int endHour = newAbsence.StartTime / 60;
//            int endMinute = newAbsence.StartTime % 60;
//            TimeOnly endTime = new TimeOnly(endHour, endMinute);



//            if (newAbsence.UserId == myId)
//            {
//                if (userType == 1)
//                {
//                    // check if i have a manager
//                    bool hasManager = await UserRelationsRepository.HasManager(myId);
                    
//                    if(hasManager)
//                    {
//                        // request
//                        return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, startTime, endTime, myId, companyId, false);
//                    }
//                    else
//                    {
//                        // add
//                        return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, startTime, endTime, myId, companyId, true);
//                    }
//                }
//                else
//                {
//                    // request
//                    return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, startTime, endTime, myId, companyId, false);
//                }
             

//            }
//            else
//            {
//                if(userType == 1)
//                {
//                    return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, startTime, endTime, myId, companyId, true);
//                }
//                else if(userType == 2)
//                {
//                    bool isSubordinate = await UserRelationsRepository.IsRelated(myId, newAbsence.UserId);
                    
//                    if(isSubordinate)
//                    {
//                        return await EmployeeAbsenceRepository.AddOrRequestAbsence(newAbsence, startDate, endDate, startTime, endTime, myId, companyId, true);
//                    }
//                    else
//                    {
//                        // error
//                        throw new Exception("Can not add absence for user who is not your subordinate");
//                    }
//                }
//                else
//                {
//                    // error
//                    throw new Exception("You do not have permissions to add absences for other users");
//                }


             
//            }


//        }
//    }
//}
