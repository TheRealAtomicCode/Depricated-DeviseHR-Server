using System;
using System.Collections.Generic;

namespace DeviseHR_Server.Models;

public partial class Note
{
    public int Id { get; set; }

    public int? Operatorid { get; set; }

    public int? Companyid { get; set; }

    public string? Notecontent { get; set; }

    public DateTime Createdat { get; set; }

    public virtual Operator? Operator { get; set; }
}
