﻿using System;
using System.Collections.Generic;

namespace DeviseHR_Server.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Title { get; set; }

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly AnnualLeaveStartDate { get; set; }

    public string? ProfilePicture { get; set; }

    public bool IsTerminated { get; set; }

    public bool IsVerified { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<string>? RefreshTokens { get; set; }

    public string? NiNo { get; set; }

    public string? DriversLicenceNumber { get; set; }

    public DateOnly? DriversLicenceExpirationDate { get; set; }

    public string? PassportNumber { get; set; }

    public DateOnly? PassportExpirationDate { get; set; }

    public bool EnableReminders { get; set; }

    public bool EnableBirthdayReminder { get; set; }

    public bool EnableReceiveRequests { get; set; }

    public bool EnableReceiveRequestsFromMyDepartment { get; set; }

    public DateOnly? ContractedLeaveStartDate { get; set; }

    public int AddedByOperator { get; set; }

    public int AddedByUser { get; set; }

    public int? UpdatedByOperator { get; set; }

    public int? UpdatedByUser { get; set; }

    public string? VerificationCode { get; set; }

    public short? LoginAttempt { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public DateTime? LastActiveTime { get; set; }

    public int UserType { get; set; }

    public int? CompanyId { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual Company? Company { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<User> Managers { get; set; } = new List<User>();

    public virtual ICollection<User> Subordinates { get; set; } = new List<User>();
}
