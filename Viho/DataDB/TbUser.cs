using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbUser
{
    public int UId { get; set; }

    public string UUsername { get; set; } = null!;

    public string UPass { get; set; } = null!;

    public string UPhone { get; set; } = null!;

    public string UEmail { get; set; } = null!;

    public int URoleid { get; set; }

    public virtual TbInvestor? TbInvestor { get; set; }

    public virtual TbRole? URole { get; set; } = null!;
}
