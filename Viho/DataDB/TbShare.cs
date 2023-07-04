using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbShare
{
    public int ShareId { get; set; }

    public double ShareAmount { get; set; }

    public DateTime ShareDate { get; set; }

    public int ShareInvestor { get; set; }

    public virtual TbInvestor? ShareInvestorNavigation { get; set; } = null!;
}
