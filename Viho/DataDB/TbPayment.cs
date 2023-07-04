using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbPayment
{
    public int PId { get; set; }

    public int PTenantid { get; set; }

    public DateTime PDate { get; set; }

    public double PAmount { get; set; }

    public string PType { get; set; } = null!;

    public int? PRoomNo { get; set; }

    public string? PLocationCode { get; set; }

    public string? PReceipt { get; set; }

    public virtual TbTenant? PTenant { get; set; } = null!;

    public virtual ICollection<TbSale> TbSales { get; set; } = new List<TbSale>();
}
