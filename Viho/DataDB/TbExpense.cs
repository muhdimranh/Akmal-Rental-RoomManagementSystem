using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbExpense
{
    public int EId { get; set; }

    public string ECat { get; set; } = null!;

    public DateTime EDate { get; set; }

    public double EAmount { get; set; }

    public string EDetail { get; set; } = null!;

    public virtual ICollection<TbSale> TbSales { get; set; } = new List<TbSale>();
}
