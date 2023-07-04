using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbAttendanceCleaner
{
    public int AttId { get; set; }

    public int AttLid { get; set; }

    public int? AttCount { get; set; }

    public virtual TbLocation? AttL { get; set; } = null!;

    public virtual ICollection<TbAttendancedate> TbAttendancedates { get; set; } = new List<TbAttendancedate>();
}
