using System;
using System.Collections.Generic;

namespace WEBSV5TOT.Models;

public partial class Class
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
