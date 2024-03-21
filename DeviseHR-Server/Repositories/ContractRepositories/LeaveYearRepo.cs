using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviseHR_Server.Repositories.ContractRepositories
{
    public static class LeaveYearRepo
    {

        public static LeaveYear? GetCurrentYeaveYearFromListOrDefault(List<LeaveYear> leaveYears, DateOnly selectedDate)
        {
            LeaveYear? currentLeaveYear = leaveYears
               .Where(ly => ly.LeaveYearStartDate <= selectedDate)
               .OrderByDescending(ly => ly.LeaveYearStartDate)
               .FirstOrDefault();

            if (currentLeaveYear == null) return null!;

            return currentLeaveYear;
        }

        public static LeaveYear GetCurrentYeaveYearFromList(List<LeaveYear> leaveYears, DateOnly selectedDate, int userId)
        {
            LeaveYear? currentLeaveYear = leaveYears
               .Where(ly => ly.LeaveYearStartDate <= selectedDate && ly.UserId == userId)
               .OrderByDescending(ly => ly.LeaveYearStartDate)
               .FirstOrDefault();

            if (currentLeaveYear == null) throw new Exception("Unable to locate leave year");

            return currentLeaveYear;
        }


        public static async Task<LeaveYear?> GetCurrentYeaveYearFromDbOrDefault(DeviseHrContext db, DateOnly selectedDate, int userId)
        {
            LeaveYear? currentLeaveYear = await db.LeaveYears
               .Where(ly => ly.LeaveYearStartDate <= selectedDate && ly.UserId == userId)
               .OrderByDescending(ly => ly.LeaveYearStartDate)
               .FirstOrDefaultAsync();

            if (currentLeaveYear == null) return null!;

            return currentLeaveYear;
        }

        public static async Task<LeaveYear> GetCurrentYeaveYearFromDb(DeviseHrContext db, DateOnly selectedDate, int userId)
        {
            LeaveYear? currentLeaveYear = await db.LeaveYears
               .Where(ly => ly.LeaveYearStartDate <= selectedDate && ly.UserId == userId)
               .OrderByDescending(ly => ly.LeaveYearStartDate)
               .FirstOrDefaultAsync();

            if (currentLeaveYear == null) throw new Exception("Unable to locate leave year");

            return currentLeaveYear;
        }


        public static async Task<List<LeaveYear>> GetLeaveYearsByUserIdFromDb(DeviseHrContext db, int userId)
        {
            List<LeaveYear> leaveYears = await db.LeaveYears
               .Where(ly => ly.UserId == userId)
               .ToListAsync();

            return leaveYears;
        }

        public static void AddMissingLeaveYears(DeviseHrContext db, List<LeaveYear> leaveYears, Contract newContract, DateOnly leaveYearStartDate, DateOnly CurrentYear, int myId)
        {
            if (leaveYears.Count == 0)
            {
                int year = (int)newContract.StartDate.Year; // Assuming you have the year stored in newLeaveYear.LeaveYearYear
                leaveYearStartDate = new DateOnly(year, leaveYearStartDate.Month, leaveYearStartDate.Day);
                int index = 0;
                // make the first leave year

                while (leaveYearStartDate <= CurrentYear.AddYears(1))
                {
                    LeaveYear newLeaveYear = new LeaveYear
                    {
                        UserId = newContract.UserId,
                        CompanyId = newContract.CompanyId,
                        LeaveYearStartDate = leaveYearStartDate,
                        TotalLeaveEntitlement = newContract.ContractedLeaveEntitlement,
                        FullLeaveYearEntitlement = newContract.CompanyLeaveEntitlement,
                        TotalLeaveAllowance = index == 0 ? newContract.ThisContractsLeaveAllowence : newContract.ContractedLeaveEntitlement,
                        NextLeaveYearEntitlement = newContract.ContractedLeaveEntitlement,
                        AddedBy = myId,
                        LeaveYearYear = leaveYearStartDate.Year,
                        IsDays = newContract.IsLeaveInDays,
                    };


                    leaveYearStartDate = leaveYearStartDate.AddYears(1);
                    leaveYears.Add(newLeaveYear);
                    index++;
                }


            }
            else
            {
                // add to existing leave years
                throw new Exception("developer error, can not add more contracts");
            }

        }


        //public static async Task<List<LeaveYear>> AddLeaveYearsFromFirstContract(DeviseHrContext db, DateOnly leaveYearStartDate, Contract firstContract, int myId)
        //{
        //    DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now.Date);
        //    DateOnly currentDatePlusOneYear = currentDate.AddYears(1);

        //    if (leaveYearStartDate < currentDate)
        //    {

        //        while (leaveYearStartDate < currentDate)
        //        {
        //            int TotalLeaveAllowance = 0;
        //            if (leaveYearStartDate.Year == currentDate.Year)
        //            {
        //                TotalLeaveAllowance = firstContract.ThisContractsLeaveAllowence;
        //            }else if(leaveYearStartDate.Year < currentDate.Year && firstContract.EndDate != null)
        //            {
        //                if(firstContract.EndDate?.Year > currentDate.Year)
        //                {
        //                    TotalLeaveAllowance = 0;
        //                }else if(firstContract.EndDate?.Year == currentDate.Year)
        //                {
        //                    // calculate
        //                    TotalLeaveAllowance = 99999999;
        //                    //
        //                    ///
        //                    //
        //                    //
        //                    //
        //                    //
        //                }
        //                else
        //                {
        //                    TotalLeaveAllowance = firstContract.ThisContractsLeaveAllowence;
        //                }
        //            }


        //            LeaveYear newLeaveYear = new LeaveYear
        //            {
        //                UserId = firstContract.UserId,
        //                CompanyId = firstContract.CompanyId,
        //                LeaveYearStartDate = leaveYearStartDate,
        //                TotalLeaveEntitlement = firstContract.CompanyLeaveEntitlement,
        //                FullLeaveYearEntitlement = ,
        //                TotalLeaveAllowance = firstContract.ThisContractsLeaveAllowence,
        //                AddedBy = myId,
        //                LeaveYearYear = leaveYearStartDate.Year,
        //                IsDays = firstContract.IsLeaveInDays,
        //            };

        //            leaveYearStartDate = leaveYearStartDate.AddYears(1);

        //            db.LeaveYears.Add(newLeaveYear);

        //        }

        //        await db.SaveChangesAsync();
        //    }
        //}


        //public static LeaveYear CalculateLeaveYearInDays(List<Contract> contracts, LeaveYear leaveYear)
        //{
        //    List<Contract> contractsCopy = new List<Contract>();
        //    foreach (Contract contract in contracts)
        //    {
        //        contractsCopy.Add(contract);
        //    }

        //    if (contractsCopy[0].StartDate < leaveYear.LeaveYearStartDate)
        //    {
        //        contractsCopy[0].StartDate = leaveYear.LeaveYearStartDate;
        //    }

        //    if (contractsCopy[contractsCopy.Count - 1].EndDate > leaveYear.LeaveYearStartDate.AddYears(1)) 
        //    {
        //        contractsCopy[contractsCopy.Count - 1].EndDate = leaveYear.LeaveYearStartDate.AddYears(1);
        //    }

        //    double leaveSum = 0;
        //    foreach (Contract contract in contractsCopy)
        //    {
        //        double contractedDaysOverCompanyDays = contract.ContractedDaysPerWeekInHalfs / contract.CompanyDaysPerWeekInHalfs;

        //        int daysBetweenContract = 1;

        //        if (contract.EndDate != null)
        //        {
        //            DateTime startDate = contract.StartDate.ToDateTime(new TimeOnly(0, 0, 0));
        //            DateTime endDate = contract.EndDate.Value.ToDateTime(new TimeOnly(0, 0, 0)); ;

        //            TimeSpan duration = endDate - startDate;
        //            daysBetweenContract = duration.Days;


        //        }
        //        else
        //        {
        //            DateTime startDate = contract.StartDate.ToDateTime(new TimeOnly(0, 0, 0));
        //            DateTime endDate = new DateTime(leaveYear.LeaveYearStartDate.AddYears(1), new TimeOnly(0, 0, 0));

        //            TimeSpan duration = endDate - startDate;
        //            daysBetweenContract = duration.Days;


        //        }

        //        double contractsLeaveEntitlement = contractedDaysOverCompanyDays / daysBetweenContract;

        //        leaveSum += contractsLeaveEntitlement;
        //    }

        //    leaveYear.FullLeaveYearEntitlement = (int)Math.Ceiling(leaveSum);
        //    leaveYear.TotalLeaveEntitlement = (int)Math.Ceiling(leaveSum);
        //}

    }
}
