

using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.ContractRepositories;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Security.Claims;

namespace DeviseHR_Server.Services.ContractServices
{
    public class ManagerContractService
    {

        public static async Task<Contract> AddContract(CreateContractRequest newContract, int myId, int companyId)
        {

            DateOnly startDate;
            DateOnly? endDate;

            if (!DateOnly.TryParse(newContract.StartDate, out startDate))
            {
                throw new ArgumentException("Invalid date format for StartDate.");
            }

            if (!DateOnly.TryParse(newContract.EndDate, out var parsedEndDate))
            {
                endDate = null;
            }
            else
            {
                endDate = parsedEndDate;
            }

            var contract = new Contract
            {
                UserId = newContract.UserId,
                PatternId = newContract.PatternId,
                ContractType = newContract.ContractType,
                StartDate = startDate,
                EndDate = endDate,
                ContractedWorkingHoursPerWeekInMinutes = newContract.ContractedWorkingHoursPerWeekInMinutes,
                FullTimeWorkingHoursPerWeekInMinutes = newContract.FullTimeWorkingHoursPerWeekInMinutes,
                ContractedWorkingDaysPerWeek = newContract.ContractedWorkingDaysPerWeek,
                AverageWorkingDay = newContract.AverageWorkingDay,
                IsLeaveInDays = newContract.IsLeaveInDays,
                CompaniesFullTimeAnnualLeaveEntitlement = newContract.CompaniesFullTimeAnnualLeaveEntitlement,
                ContractedAnnualLeaveEntitlement = newContract.ContractedAnnualLeaveEntitlement,
                ThisYearsAnnualLeaveAllowence = newContract.ThisYearsAnnualLeaveAllowance,
                TermTimeId = newContract.TermTimeId,
                AddedBy = myId,
                UpdatedBy = 0,
                CompanyId = companyId
            };
            Contract addedContract = await ManageContractRepository.AddContract(contract);

            return addedContract;
        }
    }
}
