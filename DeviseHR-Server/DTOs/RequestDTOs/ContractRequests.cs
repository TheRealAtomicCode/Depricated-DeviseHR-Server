namespace DeviseHR_Server.DTOs.RequestDTOs
{
    // Regular 3,
    // Variable 2,
    // Agency 1

    public class CreateContractRequest
    {
        public int UserId { get; set; }
        public int? PatternId { get; set; } = null;
        public int ContractType { get; set; } = 1;
        public string StartDate { get; set; } = string.Empty;
        public string? EndDate { get; set; } = null;
        public bool IsLeaveInDays { get; set; } = false;
        public int ContractedHoursPerWeekInMinutes { get; set; } = 0;
        public int ContractedDaysPerWeekInHalfs { get; set; } = 0;
        public int CompanyHoursPerWeekInMinutes { get; set; } = 0;
        public int CompanyDaysPerWeekInHalfs { get; set; } = 0;
        public int AvrageWorkingDay { get; set; } = 0;
        public int CompanyLeaveEntitlement { get; set; } = 0;
        public int ContractedLeaveEntitlement { get; set; } = 0;
        public int ThisContractsLeaveAllowence { get; set; } = 0;
        public int? TermTimeId { get; set; } = null;
    }


    public class CreateContractRequest2
    {
        public int UserId { get; set; }
        public int? PatternId { get; set; }
        public int ContractType { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string? EndDate { get; set; }
        public int ContractedWorkingHoursPerWeekInMinutes { get; set; }
        public int FullTimeWorkingHoursPerWeekInMinutes { get; set; }
        public int ContractedWorkingDaysPerWeek { get; set; }
        public int AverageWorkingDay { get; set; }
        public bool IsLeaveInDays { get; set; }
        public int CompaniesFullTimeAnnualLeaveEntitlement { get; set; }
        public int ContractedAnnualLeaveEntitlement { get; set; }
        public int ThisYearsAnnualLeaveAllowance { get; set; }
        public int? TermTimeId { get; set; }
    }




}
