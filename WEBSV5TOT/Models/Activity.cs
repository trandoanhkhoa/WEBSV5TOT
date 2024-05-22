using System;
using System.Collections.Generic;

namespace WEBSV5TOT.Models;

public partial class Activity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public long? Quantity { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Student5Good> Student5Goods { get; set; } = new List<Student5Good>();
}
