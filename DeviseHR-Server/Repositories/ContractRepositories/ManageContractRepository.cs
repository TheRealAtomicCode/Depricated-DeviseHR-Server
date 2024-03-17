using DeviseHR_Server.DTOs.RepoToServiceDTOs;
using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using Contract = DeviseHR_Server.Models.Contract;

namespace DeviseHR_Server.Repositories.ContractRepositories
{
    public class ManageContractRepository
    {

        public static async Task<Contract> AddContract(Contract contract)
        {
            var db = new DeviseHrContext();

            db.Contracts.Add(contract);
            await db.SaveChangesAsync();

            return contract;
        }


        public static async Task EndLastContractRepo(int userId, string endDate, int myId, int companyId, int userType)
        {
            using (var dbContext = new DeviseHrContext())
            {
                await dbContext.Database.ExecuteSqlRawAsync("SELECT * FROM update_last_contract_end_date({0}, {1}, {2}, {3}, {4})", userId, myId, companyId, endDate, userType);
            }
        }



        public static async Task<List<LeaveYear>> GetLeaveYearRepo(int userId, int companyId, int myId, bool checkIfSubordinate)
        {
            var db = new DeviseHrContext();

            if (checkIfSubordinate)
            {
                var hierarchy = await db.Hierarchies.FirstOrDefaultAsync(h => h.ManagerId == myId && h.SubordinateId == userId);
                if (hierarchy != null)
                {
                    throw new Exception("You are not this user's manager");
                }
            }

            List<LeaveYear> leaveYears = await db.LeaveYears.Where(ly => ly.UserId == userId && ly.CompanyId == companyId).ToListAsync();

            int leaveYearCount = leaveYears.Count;

            if (leaveYearCount == 0)
            {
                throw new Exception("Please create a contract");
            }

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now.Date);
            DateOnly currentDatePlusOneYear = currentDate.AddYears(1);

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
                        TotalLeaveEntitlement = leaveYears[leaveYears.Count - 1].FullLeaveYearEntitlement,
                        FullLeaveYearEntitlement = leaveYears[leaveYears.Count - 1].FullLeaveYearEntitlement,
                        TotalLeaveAllowance = leaveYears[leaveYears.Count - 1].FullLeaveYearEntitlement,
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
                .Include(u => u.UserId == userId)
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

            User user = await db.Users.FirstAsync<User>(u => u.Id == userId);

            CalculateLeaveYearDtoFromRepoToService dto = new CalculateLeaveYearDtoFromRepoToService();
            dto.CurrentLeaveYear = currentLeaveYear;
            dto.LeaveYears = leaveYears;
            dto.User = user;
            dto.Contracts = contracts;

            return dto;
        }


    }
}
