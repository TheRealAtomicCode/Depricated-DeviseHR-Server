using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviseHR_Server.Repositories.AbsenceRepositories
{
    public class EmployeeAbsenceRepository
    {

        public static async Task AddOrRequestAbsence(AddAbsenceRequest newAbsence, int myId, int companyId, int userType)
        {
            var db = new DeviseHrContext();

            Contract? contract = await db.Contracts.FirstOrDefaultAsync(c => c.StartDate <= newAbsence.StartDate && c.EndDate >= newAbsence.EndDate);

            if(contract == null)
            {
                throw new Exception("Can not add absence due to contract not existing or absence spans multiple contracts");
            }

            // Create a new Absence object based on the request
            var absence = new Absence
            {
                UserId = newAbsence.UserId,
                StartDate = newAbsence.StartDate,
                EndDate = newAbsence.EndDate,
                IsFirstHalfDay = newAbsence.IsFirstHalfDay,
                
                AbsenceTypes = newAbsence.AbsenceTypes,
                Comments = newAbsence.Comments,
                Approved = myId,
                CompanyId = companyId,
          
            };

            // Add the Absence to the database
            db.Absences.Add(absence);
            await db.SaveChangesAsync();
        }


    }
}
