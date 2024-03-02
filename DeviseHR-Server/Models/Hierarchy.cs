using System;
using System.Collections.Generic;

namespace DeviseHR_Server.Models;

public partial class Hierarchy
{
    public int Id { get; set; }

    public int ManagerId { get; set; }

    public int SubordinateId { get; set; }

    public virtual User Manager { get; set; } = null!;

    public virtual User Subordinate { get; set; } = null!;
}
