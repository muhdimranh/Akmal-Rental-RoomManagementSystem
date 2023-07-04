using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbInventory
{
    public int IId { get; set; }

    public int ILocationid { get; set; }

    public string IItem { get; set; } = null!;

    public int IQuantity { get; set; }

    public virtual TbLocation? ILocation { get; set; } = null!;
}
