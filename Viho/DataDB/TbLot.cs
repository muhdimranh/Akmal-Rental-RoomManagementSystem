using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbLot
{
    public int LotId { get; set; }

    public string LotName { get; set; } = null!;

    public string LotStatus { get; set; } = null!;

    public virtual ICollection<TbInvestor> TbInvestors { get; set; } = new List<TbInvestor>();
}
