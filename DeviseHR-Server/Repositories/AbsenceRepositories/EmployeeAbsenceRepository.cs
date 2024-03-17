using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviseHR_Server.Repositories.AbsenceRepositories
{
    public class EmployeeAbsenceRepository
    {

        public static async Task<Absence> AddOrRequestAbsence(AddAbsenceRequest newAbsence, DateOnly startDate, DateOnly endDate, int myId, int companyId, bool isApproved)
        {
            var db = new DeviseHrContext();

            LeaveYear? leaveYear = await db.LeaveYears.FirstOrDefaultAsync(ly => ly.LeaveYearStartDate >= startDate && ly.LeaveYearStartDate.AddYears(1) <= endDate);

            if(leaveYear == null)
            {
                throw new Exception("Can not add absence due to contract not existing or absence spans multiple contracts");
            }

            int? approved = isApproved == true ? myId : null;

            // Create a new Absence object based on the request
            var absence = new Absence
            {
                UserId = newAbsence.UserId,
                StartDate = startDate,
                EndDate = endDate,
                IsFirstHalfDay = newAbsence.IsFirstHalfDay,
                StartTime = newAbsence.StartTime, 
                EndTime = newAbsence.EndTime,
                AbsenceTypes = newAbsence.AbsenceTypes,
                Comments = newAbsence.Comments,
                Approved = approved,
                AddedBy = myId,
                CompanyId = companyId,
                LeaveIsInDays = leaveYear.IsDays
            };

            // Add the Absence to the database
            db.Absences.Add(absence);
            await db.SaveChangesAsync();

            return absence;
        }


    }
}
