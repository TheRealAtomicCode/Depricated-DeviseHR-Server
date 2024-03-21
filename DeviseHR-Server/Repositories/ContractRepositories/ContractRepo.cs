using DeviseHR_Server.DTOs.RepoToServiceDTOs;
using DeviseHR_Server.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using Contract = DeviseHR_Server.Models.Contract;

namespace DeviseHR_Server.Repositories.ContractRepositories
{
    public class ContractRepo
    {

        public static async Task<List<Contract>> GetContractsByUserIdFromDb(DeviseHrContext db, int userId)
        {
            List<Contract> contracts = await db.Contracts.Where(c => c.UserId == userId).ToListAsync();

            return contracts;
        }

        public static async Task<Contract> UpdatePreviousContracts(DeviseHrContext db, Contract contract)
        {



            return contract;
        }



        public static async Task<List<Contract>> GetContractsByLeaveYearDurationFromDb(DeviseHrContext db, DateOnly selectedDate, int userId, int companyId)
        {
            List<Contract> contracts = await db.Contracts
                    .Where(c => (c.UserId == userId && c.CompanyId == companyId && c.StartDate >= selectedDate && c.StartDate < selectedDate.AddYears(1))
                             || (c.UserId == userId && c.CompanyId == companyId && c.EndDate >= selectedDate && c.EndDate < selectedDate.AddYears(1)))
                    .ToListAsync();

            if (contracts.Count == 0) throw new Exception("Unable to locate Contracts");

            return contracts;
        }



        public static async Task<List<LeaveYear>> AddMissingLeaveYearRepo(DeviseHrContext db, List<LeaveYear> leaveYears, int userId, int companyId)
        {
            int leaveYearCount = leaveYears.Count;

            if (leaveYearCount == 0) throw new Exception("Please create a contract");

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now.Date);

            // Create all the leave years between the leave year start and the next year
            DateOnly leaveYearStartDate = leaveYears[leaveYearCount - 1].LeaveYearStartDate;

            if (leaveYearStartDate < currentDate)
            {


                while (leaveYearStartDate < currentDate)
                {
                    leaveYearStartDate = leaveYearStartDate.AddYears(1);

                    LeaveYear newLeaveYear = new LeaveYear
                    {
                        UserId = userId,
                        CompanyId = companyId,
                        LeaveYearStartDate = leaveYearStartDate,
                        TotalLeaveEntitlement = leaveYears[leaveYears.Count - 1].NextLeaveYearEntitlement,
                        FullLeaveYearEntitlement = leaveYears[leaveYears.Count - 1].NextLeaveYearEntitlement,
                        TotalLeaveAllowance = leaveYears[leaveYears.Count - 1].NextLeaveYearEntitlement,
                        AddedBy = leaveYears[leaveYears.Count - 1].AddedBy,
                        LeaveYearYear = leaveYearStartDate.Year,
                        IsDays = leaveYears[leaveYears.Count - 1].IsDays,
                    };

                    db.LeaveYears.Add(newLeaveYear);

                }

                await db.SaveChangesAsync();
            }

            return leaveYears;
        }


        public static async Task<CalculateLeaveYearDtoFromRepoToService> GetContractAndLeaveYearInfoForCalculation(int userId, DateOnly newContractStartDate)
        {
            var db = new DeviseHrContext();

            List<Contract> contracts = new List<Contract>();

            List<LeaveYear> leaveYears = await db.LeaveYears
                .Where(ly => ly.UserId == userId)
                .ToListAsync();


            LeaveYear? currentLeaveYear = leaveYears.FirstOrDefault(ly =>
                ly.LeaveYearStartDate <= newContractStartDate && newContractStartDate <= ly.LeaveYearStartDate.AddYears(1));

            if (currentLeaveYear != null)
            {
                contracts = await db.Contracts
                    .Where(c => (c.StartDate >= currentLeaveYear.LeaveYearStartDate && c.StartDate < currentLeaveYear.LeaveYearStartDate.AddYears(1))
                             || (c.EndDate >= currentLeaveYear.LeaveYearStartDate && c.EndDate < currentLeaveYear.LeaveYearStartDate.AddYears(1)))
                    .ToListAsync();

            }

            User? user = await db.Users.FirstOrDefaultAsync<User>(u => u.Id == userId);

            if (user == null) throw new Exception("Can not calculate leave for a user that does not exist.");

            CalculateLeaveYearDtoFromRepoToService dto = new CalculateLeaveYearDtoFromRepoToService();
            dto.CurrentLeaveYear = currentLeaveYear;
            dto.LeaveYears = leaveYears;
            dto.User = user;
            dto.Contracts = contracts;

            return dto;
        }


    }
}
