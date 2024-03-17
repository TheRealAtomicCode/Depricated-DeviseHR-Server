using System;
using System.Collections.Generic;

namespace DeviseHR_Server.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LicenceNumber { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public DateOnly AnnualLeaveStartDate { get; set; }

    public string? Logo { get; set; }

    public bool EnableSemiPersonalInformation { get; set; }

    public bool EnableShowEmployees { get; set; }

    public bool EnableToil { get; set; }

    public bool EnableOvertime { get; set; }

    public bool EnableAbsenceConflictsOutsideDepartments { get; set; }

    public bool EnableCarryover { get; set; }

    public bool EnableSelfCancelLeaveRequests { get; set; }

    public bool EnableEditMyInformation { get; set; }

    public bool EnableAcceptDeclineShifts { get; set; }

    public bool EnableTakeoverShift { get; set; }

    public bool EnableBroadcastShiftSwap { get; set; }

    public bool EnableRequireTwoStageApproval { get; set; }

    public string? Lang { get; set; }

    public string? Country { get; set; }

    public int? MainContactId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsSpecialClient { get; set; }

    public int MaxUsersAllowed { get; set; }

    public string? SecurityQuestionOne { get; set; }

    public string? SecurityQuestionTwo { get; set; }

    public string? SecurityAnswerTwo { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public int AddedByOperator { get; set; }

    public int? UpdatedByOperator { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<Term> Terms { get; set; } = new List<Term>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<WorkingPattern> WorkingPatterns { get; set; } = new List<WorkingPattern>();
}
