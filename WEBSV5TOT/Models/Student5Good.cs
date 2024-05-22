using System;
using System.Collections.Generic;

namespace WEBSV5TOT.Models;

public partial class Student5Good
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ActivityId { get; set; }

    public DateTime? Year { get; set; }

    public bool? Approval { get; set; }

    public string? Level { get; set; }

    public virtual Activity? Activity { get; set; }

    public virtual User? User { get; set; }
}
