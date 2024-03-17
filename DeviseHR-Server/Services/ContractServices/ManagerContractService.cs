

using DeviseHR_Server.DTOs.RepoToServiceDTOs;
using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.ContractRepositories;
using DeviseHR_Server.Repositories.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            int newContractsContractedLeaveEntitlement = (int)Math.Ceiling(newContract.ContractedLeaveEntitlement);

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
                CompanyDaysPerWeekInHalfs = newContract.CompanyDaysPerWeekInHalfs,
                AverageWorkingDay = newContract.AvrageWorkingDay,
                IsLeaveInDays = newContract.IsLeaveInDays,
                CompanyLeaveEntitlement = newContract.CompanyLeaveEntitlement,
                ContractedLeaveEntitlement = newContractsContractedLeaveEntitlement,
                ThisContractsLeaveAllowence = newContract.ThisContractsLeaveAllowence,
                TermTimeId = newContract.TermTimeId,
                AddedBy = myId,
                UpdatedBy = 0,
                CompanyId = companyId
            };

            Contract addedContract = await ManageContractRepository.AddContract(contract);
            return addedContract;
        }



        public static async Task<CreateContractRequest> CalculateLeaveYear(int userId, CreateContractRequest newContract)
        {

            if (newContract.ContractType == 1) throw new Exception("Contract does not require calculation");

            if (newContract.ContractType == 3) throw new Exception("Working Patterns need to be added first by the developers");

            if (newContract.TermTimeId != null) throw new Exception("Term times need to be added first by the developers");

            DateOnly newContractStartDate = DateOnly.FromDateTime(DateTime.ParseExact(newContract.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            List<Contract> contracts = new List<Contract>();
            double firstHalfCalculation = 0;
            double secondHalfCalculation = 0;
            DateOnly EndOfCurrentLeaveYearOrContractWhicheverIsFirst = new DateOnly();
            double result = 0;

            CalculateLeaveYearDtoFromRepoToService data = await ManageContractRepository.GetContractAndLeaveYearInfoForCalculation(userId, newContractStartDate);


            if (contracts.Count > 0)
            {
                // check if previous contracts are same unit
                if (contracts[0].IsLeaveInDays != newContract.IsLeaveInDays) throw new Exception("Can not add contract in different Leave unit as previous contract of same leave year");

                // check new contract start date is after previous contract startDate
                if (contracts[contracts.Count - 1].StartDate >= newContractStartDate) throw new Exception("New contract start date must be after previous contract start date in in order to calculate leave");

                // calculate from start of (leave year or start of contract ( if no previous contracts exist before this one ) ) to start date of new contract
                firstHalfCalculation = data.CurrentLeaveYear != null ? data.CurrentLeaveYear.FullLeaveYearEntitlement : 0;

            }



            // calculate from start of new contract to end of leave year
            if (newContract.EndDate != null && data.CurrentLeaveYear != null)
            {
                DateOnly endDate = DateOnly.ParseExact(newContract.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (endDate >= data.CurrentLeaveYear.LeaveYearStartDate.AddYears(1))
                {
                    EndOfCurrentLeaveYearOrContractWhicheverIsFirst = data.CurrentLeaveYear.LeaveYearStartDate.AddYears(1);
                }
                else
                {
                    EndOfCurrentLeaveYearOrContractWhicheverIsFirst = endDate;
                }

            }
            else
            {
                if (data.CurrentLeaveYear != null)
                {
                    EndOfCurrentLeaveYearOrContractWhicheverIsFirst = data.CurrentLeaveYear.LeaveYearStartDate.AddYears(1);
                }
                else
                {
                    // User user = await db.Users.FirstAsync<User>(u => u.Id == userId);

                    int currentYear = newContractStartDate.Year;

                    DateTime leaveYearStartDateTime = new DateTime(currentYear, data.User.AnnualLeaveStartDate.Month, data.User.AnnualLeaveStartDate.Day);
                    DateOnly leaveYearEndDate = DateOnly.FromDateTime(leaveYearStartDateTime).AddYears(1);

                    if (leaveYearEndDate < newContractStartDate)
                    {
                        EndOfCurrentLeaveYearOrContractWhicheverIsFirst = leaveYearEndDate.AddYears(1);
                    }
                    else
                    {
                        EndOfCurrentLeaveYearOrContractWhicheverIsFirst = leaveYearEndDate;
                    }


                }
            }

            string endDateString = EndOfCurrentLeaveYearOrContractWhicheverIsFirst.ToString("yyyy-MM-dd");




            if (newContract.IsLeaveInDays)
            {
                secondHalfCalculation = CalculateLeave(newContract.StartDate, endDateString, newContract.CompanyLeaveEntitlement, newContract.ContractedDaysPerWeekInHalfs, newContract.CompanyDaysPerWeekInHalfs);
                string previousEnddate = EndOfCurrentLeaveYearOrContractWhicheverIsFirst.AddYears(-1).ToString("yyyy-MM-dd");
                firstHalfCalculation = CalculatePreviousLeave(previousEnddate, newContract.StartDate, firstHalfCalculation);
                result = firstHalfCalculation + secondHalfCalculation;
                newContract.ContractedLeaveEntitlement = result;
            }
            else
            {
                secondHalfCalculation = CalculateLeave(newContract.StartDate, endDateString, newContract.CompanyLeaveEntitlement, newContract.ContractedHoursPerWeekInMinutes, newContract.CompanyHoursPerWeekInMinutes);
                string previousEnddate = EndOfCurrentLeaveYearOrContractWhicheverIsFirst.AddYears(-1).ToString("yyyy-MM-dd");
                firstHalfCalculation = CalculatePreviousLeave(previousEnddate, newContract.StartDate, firstHalfCalculation);
                result = firstHalfCalculation + secondHalfCalculation;
                newContract.ContractedLeaveEntitlement = result;
            }


            return newContract;

        }

        public static double CalculatePreviousLeave(string fromDate, string toDate, double previousLeaveEntitlement)
        {
            DateTime fromDateTime = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime toDateTime = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            int numberOfDays = toDateTime.Subtract(fromDateTime).Days;

            double result = previousLeaveEntitlement * ((double)numberOfDays / 365);

            return result;
        }

        public static double CalculateLeave(string fromDate, string toDate, int companyLeaveEntitlement, int employeeWorkingTime, int comanyWorkingTime)
        {
            double contractedToCompanyRatio = (double)employeeWorkingTime / (double)comanyWorkingTime;
            double contractedLeaveEntitlementInAYear = (double)companyLeaveEntitlement * contractedToCompanyRatio;

            DateTime fromDateTime = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime toDateTime = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            int numberOfDays = toDateTime.Subtract(fromDateTime).Days;

            double result = contractedLeaveEntitlementInAYear * ((double)numberOfDays / 365 );

            return result;
        }




        public static async Task EndLastContractService(int userId, string endDate, int myId, int companyId, int userType)
        {
            if (userType > 1 && userId == myId) throw new Exception("Managers can not end their own contracts");

            await ManageContractRepository.EndLastContractRepo(userId, endDate, myId, companyId, userType);
        }


        public static async Task<List<LeaveYear>> GetLeaveYearService(int userId, int myId, int companyId, int userType, bool enableShowEmployees)
        {
            bool checkIfSubordinate = true;
            if ((userType == 2 && enableShowEmployees == true) || userType == 1) checkIfSubordinate = false;

            List<LeaveYear> leaveYears = await ManageContractRepository.GetLeaveYearRepo(userId, companyId, myId, checkIfSubordinate);

            return leaveYears;
        }




    }
}
