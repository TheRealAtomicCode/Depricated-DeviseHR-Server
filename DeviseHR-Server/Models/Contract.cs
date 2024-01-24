using System;
using System.Collections.Generic;

namespace DeviseHR_Server.Models;

public partial class Contract
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int? PatternId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int AddedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int ContractType { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int ContractedWorkingHoursPerWeekInMinutes { get; set; }

    public int FullTimeWorkingHoursPerWeekInMinutes { get; set; }

    public int ContractedWorkingDaysPerWeek { get; set; }

    public int AverageWorkingDay { get; set; }

    public bool IsLeaveInDays { get; set; }

    public int CompaniesFullTimeAnnualLeaveEntitlement { get; set; }

    public int ContractedAnnualLeaveEntitlement { get; set; }

    public int ThisYearsAnnualLeaveAllowence { get; set; }

    public int? TermTimeId { get; set; }

    public int? DiscardedId { get; set; }

    public int? DiscardedXnumber { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual Company Company { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
