using System;
using System.Collections.Generic;

namespace DeviseHR_Server.Models;

public partial class DiscardedContract
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public string DiscardReason { get; set; } = null!;

    public DateTime DiscardAt { get; set; }

    public int DiscardBy { get; set; }

    public DateOnly FirstContractStartDate { get; set; }

    public DateOnly LastContractEndDate { get; set; }

    public int FirstContractId { get; set; }

    public int LastContractId { get; set; }
}
