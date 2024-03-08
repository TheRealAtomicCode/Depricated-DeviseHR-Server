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

    public int ContractedHoursPerWeekInMinutes { get; set; }

    public int CompanyHoursPerWeekInMinutes { get; set; }

    public int ContractedDaysPerWeekInHalfs { get; set; }

    public int AverageWorkingDay { get; set; }

    public bool IsLeaveInDays { get; set; }

    public int CompanyLeaveEntitlement { get; set; }

    public int ContractedLeaveEntitlement { get; set; }

    public int ThisContractsLeaveAllowence { get; set; }

    public int? TermTimeId { get; set; }

    public int? DiscardedId { get; set; }

    public int? DiscardedXnumber { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual Company Company { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
