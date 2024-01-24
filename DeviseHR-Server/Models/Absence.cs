using System;
using System.Collections.Generic;

namespace DeviseHR_Server.Models;

public partial class Absence
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int ContractId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool? IsFirstHalfDay { get; set; }

    public bool LeaveIsInDays { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int? DaysDeducted { get; set; }

    public int? HoursDeducted { get; set; }

    public int AbsenceTypes { get; set; }

    public string? Comments { get; set; }

    public int? Approved { get; set; }

    public int? ApprovedByAdmin { get; set; }

    public int AddedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AbsenceType AbsenceTypesNavigation { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual Contract Contract { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
