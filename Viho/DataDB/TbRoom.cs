using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbRoom
{
    public int RId { get; set; }

    public int RNo { get; set; }

    public int? RTid { get; set; }

    public int RLocationid { get; set; }

    public string RType { get; set; } = null!;

    public double RPrice { get; set; }

    public double RDepositAmount { get; set; }

    public string RDesc { get; set; } = null!;

    public string RStatus { get; set; } = null!;

    public string? RImg1 { get; set; }

    public string? RImg2 { get; set; }

    public virtual TbLocation? RLocation { get; set; } = null!;

    public virtual TbTenant? RT { get; set; }

    public virtual ICollection<TbTenant> TbTenants { get; set; } = new List<TbTenant>();
}
