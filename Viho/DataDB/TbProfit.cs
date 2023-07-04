using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbProfit
{
    public int Id { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public double TotalSales { get; set; }
}
