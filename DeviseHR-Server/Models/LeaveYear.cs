using System;
using System.Collections.Generic;

namespace DeviseHR_Server.Models;

public partial class LeaveYear
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int AddedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateOnly LeaveYearStartDate { get; set; }

    public int TotalLeaveEntitlement { get; set; }

    public int FullLeaveYearEntitlement { get; set; }

    public int TotalLeaveAllowance { get; set; }

    public bool IsDays { get; set; }

    public int? LeaveYearYear { get; set; }
}
