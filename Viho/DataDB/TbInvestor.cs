using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbInvestor
{
    public int IId { get; set; }

    public double IInvestment { get; set; }

    public double IPercent { get; set; }

    public int ILot { get; set; }

    public virtual TbUser? IIdNavigation { get; set; } = null!;

    public virtual TbLot? ILotNavigation { get; set; } = null!;

    public virtual ICollection<TbShare> TbShares { get; set; } = new List<TbShare>();
}
