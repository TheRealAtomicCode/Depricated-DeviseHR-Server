

using DeviseHR_Server.DTOs.RepoToServiceDTOs;
using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.ContractRepositories;
using DeviseHR_Server.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviseHR_Server.Services.ContractServices
{
    public static class ManageContract
    {

        public static async Task<CreateContractRequest> CalculateLeaveYear(CreateContractRequest newContract)
        {

            if (newContract.ContractType == 1) throw new Exception("Contract does not require calculation");

            if (newContract.ContractType == 3) throw new Exception("Working Patterns need to be added first by the developers");

            if (newContract.TermTimeId != null) throw new Exception("Term times need to be added first by the developers");

            DateOnly newContractStartDate = DateOnly.FromDateTime(DateTime.ParseExact(newContract.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            List<Contract> contracts = new List<Contract>();
            double firstHalfCalculation = 0;
            double secondHalfCalculation = 0;
            double fullContractCalculation = 0;
            DateOnly EndOfCurrentLeaveYearOrContractWhicheverIsFirst = new DateOnly();
            double result = 0;

            CalculateLeaveYearDtoFromRepoToService data = await ContractRepo.GetContractAndLeaveYearInfoForCalculation(newContract.UserId, newContractStartDate);


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
                secondHalfCalculation = ContractCalculate.CalculateLeave(newContract.StartDate, endDateString, newContract.CompanyLeaveEntitlement, newContract.ContractedDaysPerWeekInHalfs, newContract.CompanyDaysPerWeekInHalfs);
                string previousEnddate = EndOfCurrentLeaveYearOrContractWhicheverIsFirst.AddYears(-1).ToString("yyyy-MM-dd");
                firstHalfCalculation = ContractCalculate.CalculatePreviousLeave(previousEnddate, newContract.StartDate, firstHalfCalculation);
                result = firstHalfCalculation + secondHalfCalculation;
                fullContractCalculation = ContractCalculate.CalculateLeaveForFullYearOnContract(newContract.CompanyLeaveEntitlement, newContract.ContractedDaysPerWeekInHalfs, newContract.CompanyDaysPerWeekInHalfs);
                newContract.ContractedLeaveEntitlement =  (int)Math.Ceiling(fullContractCalculation);;
                newContract.ThisContractsLeaveAllowence = (int)Math.Ceiling(result);
            }
            else
            {
                secondHalfCalculation = ContractCalculate.CalculateLeave(newContract.StartDate, endDateString, newContract.CompanyLeaveEntitlement, newContract.ContractedHoursPerWeekInMinutes, newContract.CompanyHoursPerWeekInMinutes);
                string previousEnddate = EndOfCurrentLeaveYearOrContractWhicheverIsFirst.AddYears(-1).ToString("yyyy-MM-dd");
                firstHalfCalculation = ContractCalculate.CalculatePreviousLeave(previousEnddate, newContract.StartDate, firstHalfCalculation);
                result = firstHalfCalculation + secondHalfCalculation;
                fullContractCalculation = ContractCalculate.CalculateLeaveForFullYearOnContract(newContract.CompanyLeaveEntitlement, newContract.ContractedHoursPerWeekInMinutes, newContract.CompanyHoursPerWeekInMinutes);
                newContract.ContractedLeaveEntitlement = (int)Math.Ceiling(fullContractCalculation);
                newContract.ThisContractsLeaveAllowence = (int)Math.Ceiling(result);
            }


            return newContract;

        }


        public static async Task<Contract> AddContract(CreateContractRequest newContract, int myId, int companyId, int userType)
        {
            if (userType > 1 && newContract.UserId == myId) throw new Exception("Managers can not end their own contracts");

            DateOnly startDate;
            DateOnly? endDate;

            // parching dates
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

            //int newContractsContractedLeaveEntitlement = (int)Math.Ceiling(newContract.ContractedLeaveEntitlement);

            // adding the contract request into a contract onject
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
                ContractedLeaveEntitlement = (int)newContract.ContractedLeaveEntitlement,
                ThisContractsLeaveAllowence = newContract.ThisContractsLeaveAllowence,
                TermTimeId = newContract.TermTimeId,
                AddedBy = myId,
                UpdatedBy = 0,
                CompanyId = companyId
            };

            var db = new DeviseHrContext();

            // get all leaveyears and if any do not exist create them
            List<LeaveYear> leaveYears = await LeaveYearRepo.GetLeaveYearsByUserIdFromDb(db, newContract.UserId);
            DateOnly currentYear = new DateOnly(DateTime.Now.Year, 1, 1);

            if (leaveYears.Count == 0 || leaveYears[leaveYears.Count - 1].LeaveYearStartDate.AddYears(1).Year != currentYear.AddYears(1).Year)
            {
                // create all missing leaves
                User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == newContract.UserId);
                if (user == null) throw new Exception("Can not add contract for user that does not exist");
                 LeaveYearRepo.AddMissingLeaveYears(db, leaveYears, contract, user.AnnualLeaveStartDate, currentYear, myId);
            }

            await db.Contracts.AddAsync(contract);
            await db.LeaveYears.AddRangeAsync(leaveYears);

            await db.SaveChangesAsync();

            return contract;
        }



















        //public static async Task<Contract> AddContract(CreateContractRequest newContract, int myId, int companyId, int userType)
        //{

        //    //
        //    if (userType > 1 && newContract.UserId == myId) throw new Exception("Managers can not end their own contracts");

        //    DateOnly startDate;
        //    DateOnly? endDate;

        //    if (!DateOnly.TryParse(newContract.StartDate, out startDate))
        //    {
        //        throw new ArgumentException("Invalid date format for StartDate.");
        //    }

        //    if (!DateOnly.TryParse(newContract.EndDate, out var parsedEndDate))
        //    {
        //        endDate = null;
        //    }
        //    else
        //    {
        //        endDate = parsedEndDate;
        //    }

        //    int newContractsContractedLeaveEntitlement = (int)Math.Ceiling(newContract.ContractedLeaveEntitlement);

        //    var contract = new Contract
        //    {
        //        UserId = newContract.UserId,
        //        PatternId = newContract.PatternId,
        //        ContractType = newContract.ContractType,
        //        StartDate = startDate,
        //        EndDate = endDate,
        //        ContractedHoursPerWeekInMinutes = newContract.ContractedHoursPerWeekInMinutes,
        //        CompanyHoursPerWeekInMinutes = newContract.CompanyHoursPerWeekInMinutes,
        //        ContractedDaysPerWeekInHalfs = newContract.ContractedDaysPerWeekInHalfs,
        //        CompanyDaysPerWeekInHalfs = newContract.CompanyDaysPerWeekInHalfs,
        //        AverageWorkingDay = newContract.AvrageWorkingDay,
        //        IsLeaveInDays = newContract.IsLeaveInDays,
        //        CompanyLeaveEntitlement = newContract.CompanyLeaveEntitlement,
        //        ContractedLeaveEntitlement = newContractsContractedLeaveEntitlement,
        //        ThisContractsLeaveAllowence = newContract.ThisContractsLeaveAllowence,
        //        TermTimeId = newContract.TermTimeId,
        //        AddedBy = myId,
        //        UpdatedBy = 0,
        //        CompanyId = companyId
        //    };

        //    var db = new DeviseHrContext();

        //    List<Contract> contracts = await ContractRepo.GetContractsByUserIdFromDb(db, newContract.UserId);

        //    if (contracts.Count != 0)
        //    {
        //        int lastContractIndex = contracts.Count - 1;

        //        if (contracts[lastContractIndex].IsLeaveInDays != contract.IsLeaveInDays) throw new Exception("Can not add contract with different leave unit");

        //        if (contracts[lastContractIndex].EndDate != null)
        //        {
        //            if (contracts[lastContractIndex].EndDate > contract.StartDate) throw new Exception("New contract start date must be after the last contract end date");

        //            if (contracts[lastContractIndex].EndDate == contract.StartDate)
        //            {
        //                contracts[lastContractIndex].EndDate = contract.StartDate.AddDays(-1);
        //            }

        //        }
        //        else
        //        {
        //            contracts[lastContractIndex].EndDate = contract.StartDate.AddDays(-1);
        //        }
        //    }

        //    List<LeaveYear> leaveYears = await LeaveYearRepo.GetLeaveYearsByUserIdFromDb(db, contract.UserId);
        //    DateOnly? annualLeaveStartDate = null;

        //    if (leaveYears.Count > 0)
        //    {

        //    }
        //    else
        //    {
        //        User user = await db.Users.FirstAsync(u => u.Id == newContract.UserId);

        //        annualLeaveStartDate = user.AnnualLeaveStartDate;


        //    }

        //    // LeaveYear currentLeaveYear = LeaveYearRepo.GetCurrentYeaveYearFromList();

        //    //await ContractRepo.UpdatePreviousContracts(db, contract);

        //    //await ContractRepo.AddContract(db, contract);

        //    await db.SaveChangesAsync();

        //    return contract;
        //}





























        //public static async Task<Contract> EndLastContractService(int userId, string endDateStr, int myId, int companyId, int userType)
        //{
        //    if (userType > 1 && userId == myId) throw new Exception("Managers can not end their own contracts");

        //    bool isRelated = true;

        //    if (userType == 2) isRelated = await UserRelationsRepo.IsRelated(myId, userId);


        //    if (isRelated)
        //    {
        //        DateOnly endDate = DateOnly.Parse(endDateStr);

        //        var db = new DeviseHrContext();

        //        List<LeaveYear> leaveYears = await db.LeaveYears.Where(ly => ly.UserId == userId && ly.CompanyId == companyId).OrderBy(ly => ly.Id).ToListAsync();

        //        LeaveYear currentLeaveYear = LeaveYearRepo.GetCurrentYeaveYearFromList(leaveYears, endDate, userId);

        //        List<Contract> contracts = await ContractRepo.GetContractsByLeaveYearDurationFromDb(db, currentLeaveYear.LeaveYearStartDate, userId, companyId);

        //        int lastContractIndex = contracts.Count - 1;

        //        if (contracts[lastContractIndex].StartDate >= endDate) throw new Exception("Can not end contract before start date on previous contracts");

        //        contracts[lastContractIndex].EndDate = endDate;
        //        contracts[lastContractIndex].UpdatedBy = myId;

        //        calculate next leave years
        //        var newlyCalculatedleaveYear = LeaveYearRepo.calculateLeaveYear(contracts, currentLeaveYear);


        //        await db.SaveChangesAsync();

        //        return lastContract;
        //    }
        //    else
        //    {
        //        throw new Exception("Can not end contrats for users who are not direct subordinates");
        //    }

        //}





    }
}
