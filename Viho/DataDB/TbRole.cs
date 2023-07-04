using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbRole
{
    public int RlId { get; set; }

    public string RlDesc { get; set; } = null!;

    public virtual ICollection<TbUser> TbUsers { get; set; } = new List<TbUser>();
}
