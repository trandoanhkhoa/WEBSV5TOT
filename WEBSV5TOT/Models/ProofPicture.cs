using System;
using System.Collections.Generic;

namespace WEBSV5TOT.Models;

public partial class ProofPicture
{
    public int Id { get; set; }

    public int Student5GoodId { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime InputDate { get; set; }

    public virtual Student5Good Student5Good { get; set; } = null!;
}
