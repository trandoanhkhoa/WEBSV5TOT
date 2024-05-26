using System;
using System.Collections.Generic;

namespace WEBSV5TOT.Models;

public partial class User
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Mssv { get; set; }

    public string? Permission { get; set; }

    public int? ClassId { get; set; }

    public int? PartId { get; set; }

    public string? Avatar { get; set; }

    public string? Gender { get; set; }

    public string? Otp { get; set; }

    public int? RoleId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Part? Part { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Student5Good> Student5Goods { get; set; } = new List<Student5Good>();
}
