using System;
using System.Collections.Generic;

namespace DeviseHR_Server.Models;

public partial class Term
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public string? TermName { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int AddedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual Company? Company { get; set; }
}
