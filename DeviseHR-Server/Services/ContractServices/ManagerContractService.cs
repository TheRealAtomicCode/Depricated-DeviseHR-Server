

using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.ContractRepositories;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.Design;

using System.Globalization;
using System.Security.Claims;

namespace DeviseHR_Server.Services.ContractServices
{
    public class ManagerContractService
    {

        public static async Task<Contract> AddContract(CreateContractRequest newContract, int myId, int companyId, int userType)
        {

            if (userType > 1 && newContract.UserId == myId) throw new Exception("Managers can not end their own contracts");

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
                ContractedHoursPerWeekInMinutes = newContract.ContractedHoursPerWeekInMinutes,
                CompanyHoursPerWeekInMinutes = newContract.CompanyHoursPerWeekInMinutes,
                ContractedDaysPerWeekInHalfs = newContract.ContractedDaysPerWeekInHalfs,
                AverageWorkingDay = newContract.AvrageWorkingDay,
                IsLeaveInDays = newContract.IsLeaveInDays,
                CompanyLeaveEntitlement = newContract.CompanyLeaveEntitlement,
                ContractedLeaveEntitlement = newContract.ContractedLeaveEntitlement,
                ThisContractsLeaveAllowence = newContract.ThisContractsLeaveAllowence,
                TermTimeId = newContract.TermTimeId,
                AddedBy = myId,
                UpdatedBy = 0,
                CompanyId = companyId
            };
            Contract addedContract = await ManageContractRepository.AddContract(contract);
            return addedContract;
        }


        public static async Task EndLastContractService(int userId, string endDate, int myId, int companyId, int userType)
        {
            if (userType > 1 && userId == myId) throw new Exception("Managers can not end their own contracts");

            await ManageContractRepository.EndLastContractRepo(userId, endDate, myId, companyId, userType);

        }




    }
}
